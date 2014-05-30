using System;
using System.Net;
using CrawlLeague.ServiceInterface.Extensions;
using CrawlLeague.ServiceModel;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using CrawlLeague.ServiceModel.Util;

using ServiceStack;
using ServiceStack.OrmLite;

namespace CrawlLeague.ServiceInterface
{
    public class DivisionService : Service
    {
        public DivisionsResponse Get(FetchDivisions request)
        {
            int page = request.Page ?? 1;
            return new DivisionsResponse
            {
                Divisions = Db.Select<Division>(q => q.PageTo(page)),
                Paging = new Paging {Page = page, TotalCount = Convert.ToInt32(Db.Count<Division>())}
            };
        }

        public DivisionResponse Get(FetchDivision request)
        {
            var division = Db.SingleById<Division>(request.Id);

            if (division == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Division {0} does not exist. ".Fmt(request.Id)));

            return new DivisionResponse { Division = division };
        }

        public HttpResult Post(CreateDivision request)
        {
            var division = new Division().PopulateWith(request.SanitizeDtoHtml());

            var newId = Db.Insert(division, selectIdentity: true);

            return new HttpResult(new DivisionResponse { Division = Db.SingleById<Division>(newId) })
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                {
                    {HttpHeaders.Location, Request.AbsoluteUri.CombineWith(newId)}
                }
            };
        }

        public HttpResult Put(UpdateDivision request)
        {
            var division  = new Division().PopulateWith(request.SanitizeDtoHtml());

            int result = Db.UpdateNonDefaults(division, d => d.Id == division.Id);

            if (result == 0)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Division {0} does not exist. ".Fmt(request.Id)));

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }

        public HttpResult Delete(DeleteDivision request)
        {
            int result = Db.DeleteById<Division>(request.Id);

            if (result == 0)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Division {0} does not exist. ".Fmt(request.Id)));

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }
    }
}
