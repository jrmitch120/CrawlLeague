using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using CrawlLeague.Core.Scrapping;
using CrawlLeague.Core.Verification;
using CrawlLeague.ServiceInterface;
using CrawlLeague.ServiceInterface.RequestFilters;
using CrawlLeague.ServiceModel;
using CrawlLeague.ServiceModel.Operations;
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

        public AppHost() : base("REST Files", typeof(SeasonService).Assembly) { }
        
        public override void Configure(Container container)
        {
            //Plugins.RemoveAll(x => x is MetadataFeature); 
            Plugins.Add(new CorsFeature());
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new ValidationFeature());
            Plugins.Add(new PostmanFeature());

            var config = new AppConfig
            {
                ReadWriteApiKeys = new List<string> {ConfigurationManager.AppSettings["apiKey"]}
            };
            container.Register(config);

            GlobalRequestFilters.Add((req, res, requestDto) =>
            {
                if (!req.IsLocal && 
                    requestDto.GetType() != typeof (CreateCrawler) &&
                    req.Verb.ContainsAny(new[] {"DELETE", "PUT", "POST"}))
                {
                    var keyValidator = new ApiKeyAttribute();
                    keyValidator.Execute(req, res, requestDto);
                }
            });

            SetConfig(new HostConfig { DefaultRedirectPath = "/swagger-ui/" });

            container.RegisterAutoWiredAs<UriRequestRunner, IScraperRequestRunner>();
            container.RegisterAutoWiredAs<WebScraper, IScraper>();
            container.RegisterAutoWiredTypes(new[] {typeof (CrawlerValidator)});

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
                db.CreateTableIfNotExists<Season>();
                db.CreateTableIfNotExists<Server>();
                db.CreateTableIfNotExists<Crawler>();
                db.CreateTableIfNotExists<Division>();
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