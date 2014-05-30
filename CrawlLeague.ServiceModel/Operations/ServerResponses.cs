using System.Collections.Generic;
using CrawlLeague.ServiceModel.Types;
using CrawlLeague.ServiceModel.Util;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    public class ServersResponse : IHasResponseStatus
    {
        public List<Server> Servers { get; set; }
        public Paging Paging { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public ServersResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }

    public class ServerResponse : IHasResponseStatus
    {
        public Server Server { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public ServerResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}
