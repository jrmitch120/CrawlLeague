using System.Collections.Generic;
using CrawlLeague.ServiceModel.Util;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    public class SeasonsResponse : IHasResponseStatus
    {
        public List<Season> Seasons { get; set; }
        public Paging Paging { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public SeasonsResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }

    public class SeasonResponse : IHasResponseStatus
    {
        public Season Season { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public SeasonResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}
