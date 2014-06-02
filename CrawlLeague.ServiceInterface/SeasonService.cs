using System;
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
    public class SeasonService : Service
    {
        private readonly CrawlerValidator _validator;

        public SeasonService(CrawlerValidator validator)
        {
            _validator = validator;
        }

        private ParticipantStatusHatoes ParitipantStatusHatoes(ParticipantStatus participant)
        {
            var hatoes = new ParticipantStatusHatoes().PopulateWith(participant);

            hatoes.References.DivisionRef =
                Request.GetBaseUrl().CombineWith(new FetchDivision {Id = participant.DivisionId}.ToGetUrl());

            hatoes.References.DivisionRef =
                Request.GetBaseUrl().CombineWith(new FetchServer {Id = participant.SeasonId}.ToGetUrl());

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
            var season = new Season().PopulateWith(request.SanitizeDtoHtml());
            var newId = Db.Insert(season, selectIdentity: true);

            return new HttpResult(new SeasonResponse { Season = Db.SingleById<Season>(newId) })
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                {
                    {HttpHeaders.Location, Request.AbsoluteUri.CombineWith(newId)}
                }
            };
        }

        public HttpResult Post(CreateParticipant request)
        {
            var participant = new Participant().PopulateWith(request.SanitizeDtoHtml());

            var crawlerResp = TryResolve<CrawlerService>().Get(new FetchCrawler { Id = request.CrawlerId });
            var seasonResp = TryResolve<SeasonService>().Get(new FetchSeason { Id = request.SeasonId });

            participant.Id = crawlerResp.Crawler.Id;
            participant.DivisionId = crawlerResp.Crawler.DivisionId;

            if (!seasonResp.Season.Active)
                throw new HttpError(HttpStatusCode.BadRequest,
                    new ArgumentException("Season {0} is not currently active.".Fmt(seasonResp.Season.Id)));
            
            // Validate .rc file for the server
            var serverResp = TryResolve<ServerService>().Get(new FetchServer { Id = crawlerResp.Crawler.ServerId });

            if (!_validator.ValidateRcInit(
                    new Uri(serverResp.Server.RcUrl.Fmt("crawl-{0}".Fmt(seasonResp.Season.CrawlVersion),
                        crawlerResp.Crawler.UserName))))
                throw new HttpError(HttpStatusCode.Forbidden,
                    new ArgumentException(
                        "UserName {0} does not have a valid .rc file. ".Fmt(crawlerResp.Crawler.UserName)));


            var newId = Db.Insert(participant, selectIdentity: true);

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
