using System.Net;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    [Api("Service Description 1")]
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

    [Route("/servers", "POST", Summary = @"CREATE a new crawl server",
        Notes = "This will create a new crawl server.")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class CreateServer : Server, IReturn<ServerResponse> { }

    [Route("/servers/{Id}", "PUT", Summary = @"UPDATE a specific crawl server.",
        Notes = "This will update a crawl server.")]
    [ApiResponse(HttpStatusCode.BadRequest, "Validation error.")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class UpdateServer : Server
    {
        [ApiMember(Name = "Id", Description = "Server Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public override int Id { get; set; }
    }

    [Route("/servers/{Id}", "DELETE", Summary = @"DELETE a specific crawl server.",
        Notes = "This will delete a crawl server perminatly.")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    [ApiResponse(HttpStatusCode.NotFound, "Server was not found.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class DeleteServer
    {
        [ApiMember(Name = "Id", Description = "Server Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }
}
