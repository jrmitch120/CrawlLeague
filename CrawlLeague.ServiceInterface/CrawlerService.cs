using System;
using System.Collections.Generic;
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
            CrawlersResponse crawlersResp = Get(new FetchCrawlers { UserName = request.UserName });

            if (crawlersResp.Crawlers.Any())
                throw new HttpError(HttpStatusCode.Conflict,
                    new ArgumentException("UserName {0} already exists. ".Fmt(request.UserName)));

            var serverSrv = TryResolve<ServerService>();
            ServerResponse serverResp = serverSrv.Get(new FetchServer {Id = request.ServerId});

            if(!_validator.ValidateRcInit(new Uri(serverResp.Server.RcUrl.Fmt("crawl-git",request.UserName))))
                throw new HttpError(HttpStatusCode.Forbidden,
                    new ArgumentException("UserName {0} does not have a valid .rc file. ".Fmt(request.UserName)));

            //return null;
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
    }
}
