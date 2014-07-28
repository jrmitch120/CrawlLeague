using CrawlLeague.ServiceModel.Types;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    public class ProcessRequestResponse : IHasResponseStatus
    {
        public ProcessingRequest ProcessRequest { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public ProcessRequestResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}
