using System;
using System.Linq;
using System.Net;
using CrawlLeague.Core.Verification;
using CrawlLeague.ServiceInterface.Extensions;
using CrawlLeague.ServiceModel;
using CrawlLeague.ServiceModel.Operations;
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
        
        public CrawlersResponse Get(FetchCrawlers request)
        {
            int page = request.Page ?? 1;

            // Expression visitor to build query dynamically
            var visitor = OrmLiteConfig.DialectProvider.SqlExpression<Crawler>();

            if (request.UserName != null) // TODO Wildcard search
                visitor.Where(c => c.UserName.ToUpper() == request.UserName.ToUpper());
            
            visitor.PageTo(page);

            return new CrawlersResponse
            {
                Crawlers = Db.Select(visitor),
                Paging = new Paging {Page = page, TotalCount = Convert.ToInt32(Db.Count(visitor))}
            };
        }

        public HttpResult Post(CreateCrawler request)
        {
            // Check for existing UserName
            CrawlersResponse crawlersResp = Get(new FetchCrawlers { UserName = request.UserName });

            if (crawlersResp.Crawlers.Any())
                throw new HttpError(HttpStatusCode.Conflict,
                    new ArgumentException("UserName {0} already exists. ".Fmt(request.UserName)));

            // Check if division is open for joining
            var divisionResp = TryResolve<DivisionService>().Get(new FetchDivision { Id = request.DivisionId });

            if (!divisionResp.Division.Joinable)
                throw new HttpError(HttpStatusCode.BadGateway, 
                    new ArgumentException("Division {0} is not joinable. ".Fmt(request.UserName)));

            // Validate .rc file for the server
            var serverResp = TryResolve<ServerService>().Get(new FetchServer { Id = request.ServerId });

            if (!_validator.ValidateRcInit(new Uri(serverResp.Server.RcUrl.Fmt("crawl-git", request.UserName))))
                throw new HttpError(HttpStatusCode.Forbidden,
                    new ArgumentException("UserName {0} does not have a valid .rc file. ".Fmt(request.UserName)));

            var newId = Db.Insert((Crawler)request, selectIdentity: true);

            return new HttpResult(new CrawlerResponse { Crawler = Db.SingleById<Crawler>(newId) })
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                {
                    {HttpHeaders.Location, Request.AbsoluteUri.CombineWith(request.Id)}
                }
            };
        }

        public HttpResult Put(UpdateCrawler request)
        {
            int result = Db.Update((Crawler)request.SanitizeDtoHtml());

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
