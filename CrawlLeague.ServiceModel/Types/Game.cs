using System;
using CrawlLeague.ServiceModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    [CompositeIndex("Crawler", "SeasonId")]
    public class Game : GameCore, IAudit
    {
        [AutoIncrement]
        public int Id { get; set; }

        [Created]
        public DateTime CreatedDate { get; set; }

        [Modified]
        public DateTime ModifiedDate { get; set; }
    }

    [Alias("Game")]
    public abstract class GameCore
    {
        [ForeignKey(typeof(Crawler))]
        [Description("Id of the crawler")]
        public int CrawlerId { get; set; }

        [ForeignKey(typeof(Season))]
        [Description("Id of the Season")]
        public int SeasonId { get; set; }

        [Description("Full morgue file")]
        [CustomField("TEXT")]
        public string Morgue { get; set; }

        [Description("Crawl league calculated score")]
        public int Score { get; set; }

        [Description("When the game was completed (UTC)")]
        public DateTime CompletedDate { get; set; }
    }
}
