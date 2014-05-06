using System;
using CrawlLeague.ServiceInterface;
using CrawlLeague.ServiceModel;
using Funq;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace CrawlLeague.Api
{
    public class AppHost : AppHostBase
    {

        public AppHost() : base("REST Files", typeof(TestService).Assembly) { }
        
        public override void Configure(Container container)
        {
            Plugins.Add(new CorsFeature());

            SetConfig(new HostConfig
            {
                DebugMode = true,
            });

            var config = new AppConfig(new AppSettings());
            
            container.Register(config);

            container.Register<IDbConnectionFactory>(
                new OrmLiteConnectionFactory(
                "Server=127.0.0.1;Port=5432;User Id=postgres;Password=test123;Database=testDb;Pooling=true;MinPoolSize=0;MaxPoolSize=200",
                PostgreSqlDialect.Provider));

            using (var db = container.Resolve<IDbConnectionFactory>().Open())
            {
                db.DropAndCreateTable<Test>();
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