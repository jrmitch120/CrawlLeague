using System;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    public class Server : IAudit
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        public string Name { get; set; }

        [Index(Unique=true)]
        public string Abbreviation { get; set; }

        public string Address { get; set; }

        public string RcUrl { get; set; }

        public string MorgueUrl { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
