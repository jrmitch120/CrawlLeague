using System.Collections.Generic;
using ServiceStack.Configuration;

namespace CrawlLeague.ServiceInterface
{
    public class AppConfig
    {
        public AppConfig()
        {
            ReadWriteApiKeys = new List<string>();
        }

        public AppConfig(IAppSettings resources)
        {
        }

        public List<string> ReadWriteApiKeys { get; private set; }
    }
}
