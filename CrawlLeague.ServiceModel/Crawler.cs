using System;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    public class Crawler : IAudit
    {
        [AutoIncrement]
        public int Id { get; set; }
        
        public string UserName { get; set; }

        // TODO, division...

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
