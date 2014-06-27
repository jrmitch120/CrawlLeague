using System;
using System.Net;
using CrawlLeague.ServiceInterface.Extensions;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
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

            // Expression visitor to build query dynamically
            var visitor = Db.From<Season>();

            if (request.OnlyInProgress)
                visitor.Where(s => s.Start <= DateTime.UtcNow && s.End >= DateTime.UtcNow);

            if (request.NotFinal)
                visitor.Where(s => !s.Finalized);

            visitor.OrderByDescending(s => s.Start);

            var count = Db.Count(visitor);

            return new SeasonsResponse
            {
                Seasons = Db.Select(visitor.PageTo(page)),
                Paging = new Paging(Request.AbsoluteUri) { Page = page, TotalCount = count }
            };
        }

        public SeasonResponse Get(FetchSeason request)
        {
            var season = Db.SingleById<Season>(request.Id);

            if (season == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Season {0} does not exist. ".Fmt(request.Id)));

            return new SeasonResponse{Season = season};
        }

        public SeasonStatusResponse Get(FetchSeasonStatus request)
        {
            var season = Db.SingleById<Season>(request.Id);

            if (season == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Season {0} does not exist. ".Fmt(request.Id)));

            return new SeasonStatusResponse
            {
                Season = season,
                Status = new SeasonStatus {RoundInformation = season.RoundInformation()}
            };
        }

        public HttpResult Post(CreateSeason request)
        {
            var season = new Season().PopulateWith(request.SanitizeDtoHtml());
            var newId = Db.Insert(season, selectIdentity: true);

            return new HttpResult(new SeasonResponse {Season = Get(new FetchSeason {Id = (int) newId}).Season})
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                {
                    {HttpHeaders.Location, Request.AbsoluteUri.CombineWith(newId)}
                }
            };
        }

        public HttpResult Put(UpdateSeason request)
        {
            int result = Db.Update((Season)request.SanitizeDtoHtml());

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
