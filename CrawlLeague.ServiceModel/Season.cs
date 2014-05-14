using System;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    public class Season : IAudit
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        [Description("Name of the season")]
        public string Name { get; set; }
        
        [Description("Short description of the season")]
        public string Description { get; set; }

        [Description("Version of Dungeon Crawl that is being used")]
        public string CrawlVersion { get; set; }

        [Description("How many days a given round lasts before advancing")]
        public int DaysPerRound { get; set; }

        [Description("Start of the season in (UTC)")]
        public DateTime Start { get; set; }

        [Description("End of the season (UTC)")]
        public DateTime End { get; set; }
        
        [Description("When the season was created (UTC)")]
        public DateTime CreatedDate { get; set; }

        [Description("Last time the season was modified (UTC)")]
        public DateTime ModifiedDate { get; set; }
    }
}
