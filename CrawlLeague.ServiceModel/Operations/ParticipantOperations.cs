using System.Net;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Operations
{
    public interface IParticipantRequest
    {
        int SeasonId { get; set; }
    }

    [Route("/seasons/{SeasonId}/crawlers/{CrawlerId}/status", "GET", Summary = @"GET a specific participant's status.",
    Notes = "This will return status for a participant.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    [ApiResponse(HttpStatusCode.NotFound, "Season and/or Crawler was not found.")]
    public class FetchParticipantStatus : IParticipantRequest, IReturn<ParticipantResponse>
    {
        [ApiMember(Name = "SeasonId", Description = "Season Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int SeasonId { get; set; }

        [ApiMember(Name = "CrawlerId", Description = "Crawler Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int CrawlerId { get; set; }
    }

    [Route("/seasons/{SeasonId}/division/{DivisionId}/standings", "GET", Summary = @"GET the standings for a division.",
    Notes = "This will returned a paged list of seasons.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    public class FetchParticipantStatuses : IParticipantRequest, IReturn<ParticipantsResponse>
    {
        [ApiMember(Name = "SeasonId", Description = "Season Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int SeasonId { get; set; }

        [ApiMember(Name = "DivisionId", Description = "Division Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int DivisionId { get; set; }

        [ApiMember(Name = "Page", Description = "Current page", ParameterType = "query", DataType = "int", IsRequired = false)]
        public int? Page { get; set; }
        //[ApiMember(Name = "NoGameSince", Description = "Filter by participants without a game since (UTC)",
        //    ParameterType = "query", DataType = "dateTime", IsRequired = false)]
        //public DateTime NoGameSince { get; set; }
    }

    [Route("/seasons/{SeasonId}/crawlers", "POST", Summary = @"CREATE a new crawler participant for a season",
    Notes = "This will create a crawler participant for a given season.  Subject to field validation.")]
    [ApiResponse(422, "Validation error.")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class CreateParticipant : IReturn<ParticipantResponse>
    {
        [ApiMember(Name = "SeasonId", Description = "Id of the season to join", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int SeasonId { get; set; }

        [Description("Id of the crawler to join")]
        public int CrawlerId { get; set; }
    }
}
