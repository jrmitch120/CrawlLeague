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
        private ParticipantStatusHatoes ParitipantStatusHatoes(ParticipantStatus participant)
        {
            var hatoes = new ParticipantStatusHatoes().PopulateWith(participant);

            hatoes.DivisionRef = Request.GetBaseUrl().CombineWith(new FetchDivision { Id = participant.DivisionId }.ToGetUrl());
            hatoes.SeasonRef = Request.GetBaseUrl().CombineWith(new FetchServer { Id = participant.SeasonId }.ToGetUrl());

            return (hatoes);
        }

        public SeasonsResponse Get(FetchSeasons request)
        {
            int page = request.Page ?? 1;

            // Expression visitor to build query dynamically
            var visitor = OrmLiteConfig.DialectProvider.SqlExpression<Season>();

            if (request.InProgress )
                visitor.Where(s => s.Start <= DateTime.UtcNow && s.End >= DateTime.UtcNow);

            visitor.OrderByDescending(s => s.Start);

            var count = Convert.ToInt32(Db.Count(visitor));

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

        public ParticipantResponse Get(FetchParticipant request)
        {
            var jn = new JoinSqlBuilder<Crawler, Participant>();
            jn.Join<Crawler, Participant>(c => c.Id, p => p.CrawlerId,
                c => new {c.UserName},
                p => new {p.CrawlerId, p.LastGame, p.SeasonId, p.DivisionId});
            jn.Join<Participant, Division>(p => p.DivisionId, d => d.Id,
                null,
                d => new {DivisionName = d.Name});
            jn.Where<Participant>(x => x.CrawlerId == request.CrawlerId && request.SeasonId == x.SeasonId);
            
            var result = Db.Single<ParticipantStatus>(jn.ToSql());

            if (result == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("No match was found."));

            return new ParticipantResponse
            {
                ParticipantStatus = ParitipantStatusHatoes(result)
            };
        }

        public HttpResult Post(CreateSeason request)
        {
            var newId = Db.Insert((Season)request.SanitizeDtoHtml(), selectIdentity: true);

            return new HttpResult(new SeasonResponse { Season = Db.SingleById<Season>(newId) })
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                {
                    {HttpHeaders.Location, Request.AbsoluteUri.CombineWith(request.Id)}
                }
            };
        }

        public HttpResult Post(CreateParticipant request)
        {
            TryResolve<DivisionService>().Get(new FetchDivision { Id = request.DivisionId });
            TryResolve<CrawlerService>().Get(new FetchCrawler { Id = request.CrawlerId });
            var seasonResp = TryResolve<SeasonService>().Get(new FetchSeason { Id = request.SeasonId });

            if (seasonResp.Season.End < DateTime.UtcNow)
                throw new HttpError(HttpStatusCode.BadRequest,
                    new ArgumentException("Season {0} ended on {1}.".Fmt(seasonResp.Season.Id, seasonResp.Season.End)));

            if (seasonResp.Season.Start > DateTime.UtcNow)
                throw new HttpError(HttpStatusCode.BadRequest,
                    new ArgumentException("Season {0} starts on {1}.".Fmt(seasonResp.Season.Id, seasonResp.Season.Start)));

            var newId = Db.Insert(request, selectIdentity: true);

            return
                new HttpResult(new ParticipantResponse
                {
                    ParticipantStatus =
                        Get(new FetchParticipant {CrawlerId = request.CrawlerId, SeasonId = request.SeasonId})
                            .ParticipantStatus
                })
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
