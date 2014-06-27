using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using CrawlLeague.Core.Game;
using CrawlLeague.Core.Scrapping;
using CrawlLeague.Core.Verification;
using CrawlLeague.ServiceInterface;
using CrawlLeague.ServiceInterface.RequestFilters;
using CrawlLeague.ServiceModel;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using Funq;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Data;
using ServiceStack.MiniProfiler;
using ServiceStack.OrmLite;
using ServiceStack.Validation;

namespace CrawlLeague.Api
{
    public class AppHost : AppHostBase
    {

        public AppHost() : base("Crawl League API", typeof(SeasonService).Assembly) { }
        
        public override void Configure(Container container)
        {
            //Plugins.RemoveAll(x => x is MetadataFeature); 
            Plugins.Add(new CorsFeature());
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new ValidationFeature());
            Plugins.Add(new PostmanFeature());

            var config = new AppConfig();
            
            config.ReadWriteApiKeys.AddRange(ConfigurationManager.AppSettings["apiKeys"].Split(new[] {','}));
            
            container.Register(config);

            GlobalRequestFilters.Add((req, res, requestDto) =>
            {
                if (requestDto.GetType() != typeof (CreateCrawler) &&
                    req.Verb.ContainsAny(new[] {"DELETE", "PUT", "POST"}))
                {
                    var keyValidator = new ApiKeyAttribute();
                    keyValidator.Execute(req, res, requestDto);
                }
            });

            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/swagger-ui/"
            });

            container.RegisterAutoWiredAs<UriRequestRunner, IScraperRequestRunner>();
            container.RegisterAutoWiredAs<WebScraper, IScraper>();
            container.RegisterAutoWiredTypes(new[]
            {typeof (CrawlerValidator), typeof (GameRunner), typeof (GameProcessor)});

            container.RegisterAutoWired<GameEngine>().ReusedWithin(ReuseScope.Container);

            //container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));

            container.Register<IDbConnectionFactory>(
                new OrmLiteConnectionFactory(HttpContext.Current.Server.MapPath("~/App_Data/leaguedata.sqlite"),
                    SqliteDialect.Provider));

            //container.Register<IDbConnectionFactory>(
            //    new OrmLiteConnectionFactory(
            //    "Server=127.0.0.1;Port=5432;User Id=postgres;Password=test123;Database=testDb;Pooling=true;MinPoolSize=0;MaxPoolSize=200",
            //    PostgreSqlDialect.Provider));

            
            using (var db = container.Resolve<IDbConnectionFactory>().Open())
            {
                db.CreateTableIfNotExists<Participant>();
                db.CreateTableIfNotExists<Season>();
                db.CreateTableIfNotExists<Division>();
                db.CreateTableIfNotExists<Server>();
                db.CreateTableIfNotExists<Crawler>();
            }

            OrmLiteConfig.InsertFilter = (dbCmd, row) =>
            {
                var auditRow = row as IAudit;
                if (auditRow != null)
                    auditRow.CreatedDate = auditRow.ModifiedDate = DateTime.UtcNow;
            };

            OrmLiteConfig.UpdateFilter = (dbCmd, row) =>
            {
                var auditRow = row as IAudit;
                if (auditRow != null)
                    auditRow.ModifiedDate = DateTime.UtcNow;
            };
        }
    }

    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your application
            (new AppHost()).Init();
        }

        protected void Application_BeginRequest(object src, EventArgs e)
        {
            if (Request.IsLocal)
                Profiler.Start();
        }
        
        protected void Application_EndRequest(object src, EventArgs e)
        {
            Profiler.Stop();
        }
    }
}