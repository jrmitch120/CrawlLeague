using System.Net;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    [Route("/servers", "GET", Summary = @"GET a list of dungeon crawl servers.",
        Notes = "This will returned a paged list of dungeon crawl servers.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    public class FetchServers : IReturn<ServersResponse>
    {
        [ApiMember(Name = "Page", Description = "Requested page of data", ParameterType = "query", DataType = "int", IsRequired = false)]
        public int? Page { get; set; }
    }

    [Route("/servers/{Id}", "GET", Summary = @"GET a specific season.",
        Notes = "This will return a server given by Id.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    [ApiResponse(HttpStatusCode.NotFound, "Server was not found.")]
    public class FetchServer : IReturn<ServerResponse>
    {
        [ApiMember(Name = "Id", Description = "Server Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }
}
