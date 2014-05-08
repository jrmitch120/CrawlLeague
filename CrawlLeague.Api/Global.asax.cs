using System;
using System.Collections.Generic;
using System.Configuration;
using CrawlLeague.ServiceInterface;
using CrawlLeague.ServiceInterface.RequestFilters;
using CrawlLeague.ServiceInterface.Validation;
using CrawlLeague.ServiceModel;
using Funq;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Validation;

namespace CrawlLeague.Api
{
    public class AppHost : AppHostBase
    {

        public AppHost() : base("REST Files", typeof(SeasonService).Assembly) { }
        
        public override void Configure(Container container)
        {
            Plugins.RemoveAll(x => x is MetadataFeature); 
            Plugins.Add(new CorsFeature());
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new ValidationFeature());

            var config = new AppConfig
            {
                ReadWriteApiKeys = new List<string> {ConfigurationManager.AppSettings["apiKey"]}
            };
            container.Register(config);

            GlobalRequestFilters.Add((req, res, requestDto) =>
            {
                if (req.Verb.ContainsAny(new[] {"DELETE", "PUT", "POST"}))
                {
                    var keyValidator = new ApiKeyAttribute();
                    keyValidator.Execute(req, res, requestDto);
                }
            });

            SetConfig(new HostConfig { DefaultRedirectPath = "/swagger-ui/" });

            container.RegisterValidators(typeof(CreateSeasonValidator).Assembly);
            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));

            //container.Register<IDbConnectionFactory>(
            //    new OrmLiteConnectionFactory(
            //    "Server=127.0.0.1;Port=5432;User Id=postgres;Password=test123;Database=testDb;Pooling=true;MinPoolSize=0;MaxPoolSize=200",
            //    PostgreSqlDialect.Provider));

            using (var db = container.Resolve<IDbConnectionFactory>().Open())
            {
                db.DropAndCreateTable<Season>();
                db.DropAndCreateTable<Server>();
            }
        }
    }

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your application
            (new AppHost()).Init();
        }
    }
}