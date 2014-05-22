using System;
using CrawlLeague.ServiceModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    public class Crawler : IAudit
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        [Index(Unique = true)]
        [Description("Crawler's user name.  Must match a user name on an active crawl server")]
        public string UserName { get; set; }

        [ForeignKey(typeof(Server))]
        [Description("Server that the crawler is currently bound to")]
        public int ServerId { get; set; }

        [ForeignKey(typeof(Division))]
        [Description("Division that the crawler is slotted into as of the last recalculation")]
        public int DivisionId { get; set; }

        [Description("Active or inactive crawler")]
        public bool Active { get; set; }

        [Description("Has the crawler been banned")]
        public bool Banned { get; set; }

        [Description("Reason for a banning")]
        public bool BanReason { get; set; }

        [Created]
        public DateTime CreatedDate { get; set; }

        [Modified]
        public DateTime ModifiedDate { get; set; }
    }
}
