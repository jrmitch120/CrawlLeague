using System.Net;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    [Api("Service Description 1")]
    [Route("/admin/processrequests", "GET", Summary = @"GET a list of processs requests.",
        Notes = "This will returned a league processing request.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    public class FetchProcessRequest : IReturn<ProcessRequestResponse>
    {
    }
}
