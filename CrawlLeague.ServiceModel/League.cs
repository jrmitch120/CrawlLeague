using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    [Route("/leagues")]
    public class Leagues { }

    public class LeaguesResponse
    {
        public List<League> Leagues { get; set; }
    }

    [Route("/leagues/{Id}")]
    public class League : IAudit
    {
        [AutoIncrement]
        public int Id { get; set; }

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
