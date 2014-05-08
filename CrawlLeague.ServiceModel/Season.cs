using System;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    public class Season : IAudit
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CrawlVersion { get; set; }
        public int DaysPerRound { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
