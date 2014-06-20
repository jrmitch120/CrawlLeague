using System.Net;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    [Api("Service Description 1")]
    [Route("/admin/processequests", "GET", Summary = @"GET a list of processs requests.",
        Notes = "This will returned a list of process requests.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    public class FetchProcessRequests : IReturn<ProcessRequestsResponse>
    {
    }
}
