using System;
using System.Net;
using CrawlLeague.Core.Verification;
using CrawlLeague.ServiceModel;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Util;
using ServiceStack;
using ServiceStack.OrmLite;

namespace CrawlLeague.ServiceInterface
{
    public class SeasonService : Service
    {
        public SeasonsResponse Get(FetchSeasons request)
        {
            int page = request.Page ?? 1;
            return new SeasonsResponse
            {
                Seasons = Db.Select<Season>(q => q.Limit(skip: (page - 1)*Paging.PageSize, rows: Paging.PageSize)),
                Paging = new Paging {Page = page, TotalCount = Convert.ToInt32(Db.Count<Season>())}
            };
        }

        public SeasonResponse Get(FetchSeason request)
        {
            var season = Db.SingleById<Season>(request.Id);

            if (season == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Season {0} does not exist. ".Fmt(request.Id)));

            return new SeasonResponse{Season = season};
        }

        public HttpResult Post(CreateSeason request)
        {
            var newId = Db.Insert((Season)request, selectIdentity:true);

            return new HttpResult(new SeasonResponse { Season = Db.SingleById<Season>(newId) })
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                {
                    {HttpHeaders.Location, Request.AbsoluteUri.CombineWith(request.Id)}
                }
            };
        }

        public HttpResult Put(UpdateSeason request)
        {
            int result = Db.Update((Season)request);

            if (result == 0)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Season {0} does not exist. ".Fmt(request.Id)));

            return new HttpResult { StatusCode = HttpStatusCode.NoContent};
        }

        public HttpResult Delete(DeleteSeason request)
        {
            int result = Db.DeleteById<Season>(request.Id);

            if(result == 0)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Season {0} does not exist. ".Fmt(request.Id)));

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }
    }
}
