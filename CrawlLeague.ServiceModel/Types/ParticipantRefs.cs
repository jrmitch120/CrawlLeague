using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public class ParticipantRefs
    {
        [Description("Link to the crawler")]
        public string CrawlerRef { get; set; }

        [Description("Link to the server that the crawler is playing on")]
        public string ServerRef { get; set; }

        [Description("Link to the season that the crawler is bound to")]
        public string SeasonRef { get; set; }

        [Description("Link to the division that the crawler is slotted into as of the last divison realignment")]
        public string DivisionRef { get; set; }
    }
}