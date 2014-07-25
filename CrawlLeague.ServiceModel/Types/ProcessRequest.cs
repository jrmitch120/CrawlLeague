using System.Collections.Generic;

namespace CrawlLeague.ServiceModel.Types
{   
    public class ProcessingRequest
    {
        public ProcessingRequest()
        {
            SeasonProcessRequests = new List<SeasonProcessRequest>();
        }

        public List<SeasonProcessRequest> SeasonProcessRequests { get; private set; }
    }

    public class SeasonProcessRequest
    {
        public SeasonProcessRequest()
        {
            RoundProcessRequests = new List<RoundProcessRequest>();
        }

        public int SeasonId { get; set; }

        public List<RoundProcessRequest> RoundProcessRequests { get; private set; }
    }

    public class RoundProcessRequest
    {
        public RoundProcessRequest()
        {
            GameFetchRequests = new List<GameFetchRequest>();
        }

        public Round Round { get; set; }

        public List<GameFetchRequest> GameFetchRequests { get; private set; }
    }

    public class GameFetchRequest
    {
        public int CrawlerId { get; set; }
        public int ParticipantId { get; set; }
        public string MorgueUrl { get; set; }
        public int UtcOffset { get; set; }
    }
}
