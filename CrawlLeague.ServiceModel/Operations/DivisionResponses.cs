using System.Collections.Generic;
using CrawlLeague.ServiceModel.Types;
using CrawlLeague.ServiceModel.Util;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    public class DivisionsResponse : IHasResponseStatus
    {
        public List<Division> Divisions { get; set; }
        public Paging Paging { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public DivisionsResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }

    public class DivisionResponse : IHasResponseStatus
    {
        public Division Division { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public DivisionResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}
