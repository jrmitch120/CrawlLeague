using System.Net;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    [Route("/crawlers", "POST", Summary = @"CREATE a new crawler account",
        Notes = "This will create a new crawler account.")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class CreateCrawler : Server, IReturn<ServerResponse> { }
    
}
