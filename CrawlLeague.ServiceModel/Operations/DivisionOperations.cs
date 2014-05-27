﻿using System.Net;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    [Api("Service Description 1")]
    [Route("/divisions", "GET", Summary = @"GET a list of divisions.",
        Notes = "This will returned a paged list of divisions.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    public class FetchDivisions : IReturn<DivisionsResponse>
    {
        [ApiMember(Name = "Page", Description = "Current page", ParameterType = "query", DataType = "int", IsRequired = false)]
        public int? Page { get; set; }
    }

    [Route("/divisions/{Id}", "GET", Summary = @"GET a specific division.",
        Notes = "This will return a division given by Id.")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    [ApiResponse(HttpStatusCode.NotFound, "Division was not found.")]
    public class FetchDivision : IReturn<DivisionResponse>
    {
        [ApiMember(Name = "Id", Description = "Division Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }

    [Route("/divisions", "POST", Summary = @"CREATE a new division",
        Notes = "This will create a new division.  Subject to field validation.")]
    [ApiResponse(HttpStatusCode.BadRequest, "Validation error.")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class CreateDivision : Division, IReturn<DivisionResponse> { }

    [Route("/divisions/{Id}", "PUT", Summary = @"UPDATE a specific division.",
        Notes = "This will update a division.  Subject to field validation.")]
    [ApiResponse(HttpStatusCode.BadRequest, "Validation error.")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class UpdateDivision : Division
    {
        [ApiMember(Name = "Id", Description = "Division Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public override int Id { get; set; }
    }

    [Route("/divisions/{Id}", "DELETE", Summary = @"DELETE a specific division.",
        Notes = "This will delete a division perminatly.")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    [ApiResponse(HttpStatusCode.NotFound, "Division was not found.")]
    [ApiResponse(HttpStatusCode.Unauthorized, "Invalid X-ApiKey header.")]
    public class DeleteDivision
    {
        [ApiMember(Name = "Id", Description = "Division Id", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }
}
