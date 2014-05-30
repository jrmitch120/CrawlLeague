using System;
using CrawlLeague.ServiceModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    [CompositeIndex(true, "CrawlerId", "SeasonId")]
    public class Participant : ParticipantCore, IAudit
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        [Description("Curent total score for the season")]
        public DateTime Score { get; set; }

        [Created]
        public DateTime CreatedDate { get; set; }

        [Modified]
        public DateTime ModifiedDate { get; set; }
    }

    public abstract class ParticipantCore
    {
        [ForeignKey(typeof(Crawler))]
        [Description("Id of the crawler")]
        public int CrawlerId { get; set; }

        [ForeignKey(typeof(Season))]
        [Description("Id of the season")]
        public int SeasonId { get; set; }

        [ForeignKey(typeof(Division))]
        [Description("Id of the division")]
        public int DivisionId { get; set; }

        [Description("When was the last game recorded")]
        public DateTime LastGame { get; set; }
    }
}
