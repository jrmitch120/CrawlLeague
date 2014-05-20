using System.Net;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    [Route("/crawlers", "GET", Summary = @"GET a list of crawlers.",
        Notes = "This will returned a paged list of crawlers.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    public class FetchCrawlers : IReturn<CrawlersResponse>
    {
        [ApiMember(Name = "Name", Description = "Name of crawler", ParameterType = "query", DataType = "string", IsRequired = false)]
        public string Name { get; set; }
        
        [ApiMember(Name = "Page", Description = "Current page", ParameterType = "query", DataType = "int", IsRequired = false)]
        public int? Page { get; set; }
    }

    [Route("/crawlers/{Id}", "GET", Summary = @"GET a specific crawler.",
        Notes = "This will return a crawler given by Id.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    [ApiResponse(HttpStatusCode.NotFound, "Crawler was not found.")]
    public class FetchCrawler : IReturn<CrawlerResponse>
    {
        [ApiMember(Name = "Id", Description = "Crawler Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }

    [Route("/crawlers", "POST", Summary = @"CREATE a new crawler account",
        Notes = "This will create a new crawler account.")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class CreateCrawler : Server, IReturn<ServerResponse> { }
    
}
