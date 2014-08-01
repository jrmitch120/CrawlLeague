using System.Net;
using CrawlLeague.ServiceModel.Types;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    [Route("/games/{Id}", "GET", Summary = @"GET a specific game",
        Notes = "This will return a game given by Id.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    [ApiResponse(HttpStatusCode.NotFound, "Game was not found.")]
    public class FetchGame : IReturn<GameResponse>
    {
        [ApiMember(Name = "Id", Description = "Game Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }

    [Route("/seasons/{SeasonId}/crawlers/{CrawlerId}/games", "POST", Summary = @"CREATE a new game",
        Notes = "This will create a new game for a seasonal crawler.")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class CreateGame : GameCore, IReturn<GameResponse> { }
}
