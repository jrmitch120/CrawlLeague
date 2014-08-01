using CrawlLeague.ServiceModel.Types;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    public class GameResponse : IHasResponseStatus
    {
        public Game Game { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public GameResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}
