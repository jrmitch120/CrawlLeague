using System;
using CrawlLeague.ServiceModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public class Game : IAudit
    {
        [AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Participant))]
        [Description("Id of the participant")]
        public int ParticipantId { get; set; }

        [Description("Full morgue file")]
        public string Morgue { get; set; }

        [Description("Crawl league calculated score")]
        public int Score { get; set; }

        [Description("When the game was completed (UTC)")]
        public DateTime CompletedDate { get; set; }

        [Created]
        public DateTime CreatedDate { get; set; }

        [Modified]
        public DateTime ModifiedDate { get; set; }
    }
}
