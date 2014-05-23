using System.Net;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    [Api("Service Description 1")]
    [Route("/seasons", "GET", Summary = @"GET a list of seasons.",
        Notes = "This will returned a paged list of seasons.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    public class FetchSeasons : IReturn<SeasonsResponse>
    {
        [ApiMember(Name = "Page", Description = "Current page", ParameterType = "query", DataType = "int", IsRequired = false)]
        public int? Page { get; set; }
    }

    [Route("/seasons/{Id}", "GET", Summary = @"GET a specific season.",
        Notes = "This will return a season given by Id.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    [ApiResponse(HttpStatusCode.NotFound, "Season was not found.")]
    public class FetchSeason : IReturn<SeasonResponse>
    {
        [ApiMember(Name = "Id", Description = "Season Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }

    [Route("/seasons", "POST", Summary = @"CREATE a new season",
        Notes = "This will create a new season.  Subject to field validation.")]
    [ApiResponse(HttpStatusCode.BadRequest, "Validation error.")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class CreateSeason : Season, IReturn<SeasonResponse> { }

    [Route("/seasons/{Id}", "PUT", Summary = @"UPDATE a specific season.",
        Notes = "This will update a season.  Subject to field validation.")]
    [ApiResponse(HttpStatusCode.BadRequest, "Validation error.")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class UpdateSeason : Season
    {
        [ApiMember(Name = "Id", Description = "Season Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public override int Id { get; set; }
    }

    [Route("/seasons/{Id}", "DELETE", Summary = @"DELETE a specific season.",
        Notes = "This will delete a season perminatly.")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    [ApiResponse(HttpStatusCode.NotFound, "Season was not found.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class DeleteSeason
    {
        [ApiMember(Name = "Id", Description = "Season Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }
}
