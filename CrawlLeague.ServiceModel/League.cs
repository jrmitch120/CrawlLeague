using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    [Route("/leagues","GET")]
    public class Leagues: IReturn<LeagueResponse> { }

    public class LeaguesResponse : IHasResponseStatus
    {
        public List<League> Leagues { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public LeaguesResponse()
        {
            ResponseStatus = new ResponseStatus();
        }    
    }

    [Route("/leagues/{Id}", "GET,DELETE")]
    public class LeagueRequest: IReturn<LeagueResponse>
    {
        [ApiMember(Name = "Id", Description = "League Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }

    [Route("/leagues", "POST")]
    [Route("/leagues/{Id}", "PUT")]
    public class League : IAudit, IReturn<LeagueResponse>
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

    public class LeagueResponse : IHasResponseStatus
    {
        public League League { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public LeagueResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}
