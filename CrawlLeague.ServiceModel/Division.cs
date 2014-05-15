using System;
using CrawlLeague.ServiceModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    
    public class Division : IAudit
    {
        [AutoIncrement]
        public int Id { get; set; }

        [Description("Name of the division")]
        public string Name { get; set; }

        [Description("Description of the division")]
        public string Description { get; set; }

        [Created]
        public DateTime CreatedDate { get; set; }
        
        [Modified]
        public DateTime ModifiedDate { get; set; }
    }
}
