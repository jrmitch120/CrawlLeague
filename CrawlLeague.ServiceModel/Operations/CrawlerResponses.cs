using System.Collections.Generic;
using CrawlLeague.ServiceModel.Util;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    public class CrawlersResponse : IHasResponseStatus
    {
        public List<Crawler> Crawlers { get; set; }
        public Paging Paging { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public CrawlersResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }

    public class CrawlerResponse : IHasResponseStatus
    {
        public Crawler Crawler { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public CrawlerResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}
