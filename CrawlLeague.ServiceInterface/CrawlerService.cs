using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CrawlLeague.Core.Verification;
using CrawlLeague.ServiceInterface.Extensions;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using CrawlLeague.ServiceModel.Util;
using ServiceStack;
using ServiceStack.OrmLite;

namespace CrawlLeague.ServiceInterface
{
    public class CrawlerService : Service
    {
        private readonly CrawlerValidator _validator;

        public CrawlerService(CrawlerValidator validator)
        {
            _validator = validator;
        }

        private CrawlerHatoes CrawlerHatoes(Crawler crawler)
        {
            var hatoes = new CrawlerHatoes().PopulateWith(crawler);

            hatoes.References.DivisionRef =
                Request.GetBaseUrl().CombineWith(new FetchDivision {Id = crawler.DivisionId}.ToGetUrl());

            hatoes.References.ServerRef =
                Request.GetBaseUrl().CombineWith(new FetchServer {Id = crawler.ServerId}.ToGetUrl());

            return (hatoes);
        }

        public CrawlersResponse Get(FetchCrawlers request)
        {
            int page = request.Page ?? 1;

            // Expression visitor to build query dynamically
            var visitor = Db.From<Crawler>();

            if (!request.UserName.IsNullOrEmpty()) // TODO Wildcard search
                visitor.Where(c => c.UserName.ToUpper() == request.UserName.ToUpper());
            
            var count = Db.Count(visitor);
            var results = Db.Select(visitor.PageTo(page));
            var crawlers = new List<CrawlerHatoes>();
            
            results.ForEach(r => crawlers.Add(CrawlerHatoes(r)));

            return new CrawlersResponse
            {
                Crawlers = crawlers,
                Paging = new Paging(Request.AbsoluteUri) {Page = page, TotalCount = count}
            };
        }

        public CrawlerResponse Get(FetchCrawler request)
        {
            var crawler = Db.SingleById<Crawler>(request.Id);
            
            if (crawler == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Crawler {0} does not exist. ".Fmt(request.Id)));

            return new CrawlerResponse { Crawler = CrawlerHatoes(crawler) };
        }

        public HttpResult Post(CreateCrawler request)
        {
            var crawler = new Crawler().PopulateWith(request.SanitizeDtoHtml());

            // Check for existing UserName
            if (Get(new FetchCrawlers { UserName = request.UserName }).Crawlers.Any())
                throw new HttpError(HttpStatusCode.Conflict,
                    new ArgumentException("UserName {0} already exists. ".Fmt(request.UserName)));

            // Check if division is open for joining
            var divisionResp = ResolveService<DivisionService>().Get(new FetchDivision { Id = request.DivisionId });

            if (!divisionResp.Division.Joinable)
                throw new HttpError(HttpStatusCode.BadRequest, 
                    new ArgumentException("Division {0} is not joinable. ".Fmt(request.UserName)));

            // Validate .rc file for the server
            var serverResp = ResolveService<ServerService>().Get(new FetchServer { Id = request.ServerId });

            if (!_validator.ValidateRcInit(new Uri(serverResp.Server.RcUrl.Fmt("crawl-git", request.UserName))))
                throw new HttpError(HttpStatusCode.Forbidden,
                    new ArgumentException("UserName {0} does not have a valid .rc file. ".Fmt(request.UserName)));

            var newId = Db.Insert(crawler, selectIdentity: true);

            return new HttpResult(new CrawlerResponse { Crawler = CrawlerHatoes(Db.SingleById<Crawler>(newId)) })
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                {
                    {HttpHeaders.Location, Request.AbsoluteUri.CombineWith(newId)}
                }
            };
        }

        public HttpResult Put(UpdateCrawler request)
        {
            var crawler = new Crawler().PopulateWith(request.SanitizeDtoHtml());

            int result = Db.UpdateNonDefaults(crawler, c => c.Id == crawler.Id);

            if (result == 0)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Crawler {0} does not exist. ".Fmt(request.Id)));

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }

        public HttpResult Delete(DeleteCrawler request)
        {
            int result = Db.DeleteById<Crawler>(request.Id);

            if (result == 0)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Crawler {0} does not exist. ".Fmt(request.Id)));

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }
    }
}
