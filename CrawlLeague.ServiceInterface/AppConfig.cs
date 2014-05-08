using System.Collections.Generic;
using ServiceStack.Configuration;

namespace CrawlLeague.ServiceInterface
{
    public class AppConfig
    {
        public AppConfig()
        {
        }

        public AppConfig(IAppSettings resources)
        {
        }
        
        public IList<string> ReadWriteApiKeys { get; set; }
    }
}
