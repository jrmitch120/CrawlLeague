using System.Collections.Generic;
using CrawlLeague.ServiceModel.Types;
using CrawlLeague.ServiceModel.Util;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    public class ProcessRequestsResponse : IHasResponseStatus
    {
        public ProcessingRequest ProcessRequest { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public ProcessRequestsResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}
