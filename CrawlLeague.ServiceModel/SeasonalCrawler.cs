using System;
using CrawlLeague.ServiceModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    public class SeasonalCrawler : IAudit
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        [ForeignKey(typeof(Crawler))]
        [Description("Id of the crawler")]
        public int CrawlerId { get; set; }

        [ForeignKey(typeof(Season))]
        [Description("Id of the season to join")]
        public int SeasonId { get; set; }

        [ForeignKey(typeof(Division))]
        [Description("Id of the division to join")]
        public int DivisionId { get; set; }

        [Created]
        public DateTime CreatedDate { get; set; }

        [Modified]
        public DateTime ModifiedDate { get; set; }
    }
}
