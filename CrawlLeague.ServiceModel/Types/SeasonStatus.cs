using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public class SeasonStatus
    {
        [Description("Information for the season's rounds")]
        public RoundInformation RoundInformation { get; set; }
    }
}