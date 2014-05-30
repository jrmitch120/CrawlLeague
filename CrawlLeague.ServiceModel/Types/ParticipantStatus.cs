using System;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public class ParticipantStatus
    {
        [Description("Id of the crawler")]
        public int CrawlerId { get; set; }

        [Description("Id of the season")]
        public int SeasonId { get; set; }

        [Description("Id of the division that the participant belongs to")]
        public int DivisionId { get; set; }

        [Description("Name of the division that the participant belongs to")]
        public string DivisionName { get; set; }

        [Description("Name of the crawler")]
        public string UserName { get; set; }

        [Description("When was the last game recorded for this participant")]
        public DateTime LastGame { get; set; }
    }
}