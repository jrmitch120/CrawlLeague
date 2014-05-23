using System;
using CrawlLeague.ServiceModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    
    public class Division : IAudit
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        [Description("Name of the division")]
        public string Name { get; set; }

        [Description("Description of the division")]
        public string Description { get; set; }

        [Description("Can the division be joined during account creation")]
        public bool Joinable { get; set; }

        [Description("What tier the division is.  Higher is more prestigous")]
        [Index(Unique = true)]
        public int Tier { get; set; }

        [Created]
        public DateTime CreatedDate { get; set; }
        
        [Modified]
        public DateTime ModifiedDate { get; set; }
    }
}
