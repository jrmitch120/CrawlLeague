using System;
using CrawlLeague.ServiceModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public class Game : IAudit
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        [ForeignKey(typeof(Crawler))]
        [Description("Id of the crawler")]
        public int CrawlerId { get; set; }

        [ForeignKey(typeof(Season))]
        [Description("Id of the season to join")]
        public int SeasonId { get; set; }

        [Description("Full morgue file")]
        public string Morgue { get; set; }

        [Description("Crawl league calculated score")]
        public int Score { get; set; }

        [Created]
        public DateTime CreatedDate { get; set; }

        [Modified]
        public DateTime ModifiedDate { get; set; }
    }
}
