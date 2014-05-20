using System;
using System.Collections.Generic;
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
            var crawlers = new List<Crawler>();

            crawlers.AddRange(request.Name != null
                ? Db.Select<Crawler>(x => x.Where(c => c.UserName == request.Name).PageTo(page))
                : Db.Select<Crawler>(q => q.PageTo(page)));

            return new CrawlersResponse
            {
                Crawlers = crawlers,
                Paging = new Paging { Page = page, TotalCount = Convert.ToInt32(Db.Count<Crawler>()) }
            };
        }

        public HttpResult Post(CreateCrawler request)
        {
            //TODO!!
            //var serverSrv = TryResolve<ServerService>();
            _validator.ValidateRcInit(new Uri("http://crawl.berotato.org/crawl/rcfiles/crawl-0.14/shobalk.rc"));

            return null;
            //var newId = Db.Insert((Server)request, selectIdentity: true);

            //return new HttpResult(new CrawlerResponse { Crawler = Db.SingleById<Crawler>(newId) })
            //{
            //    StatusCode = HttpStatusCode.Created,
            //    Headers =
            //    {
            //        {HttpHeaders.Location, Request.AbsoluteUri.CombineWith(request.Id)}
            //    }
            //};
        }
    }
}
