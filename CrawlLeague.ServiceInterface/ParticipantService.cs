using System;
using System.Net;
using CrawlLeague.Core.Verification;
using CrawlLeague.ServiceInterface.Extensions;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace CrawlLeague.ServiceInterface
{
    public class ParticipantService : Service
    {
        private readonly CrawlerValidator _validator;

        public ParticipantService(CrawlerValidator validator)
        {
            _validator = validator;
        }

        private ParticipantStatusHatoes ParitipantStatusHatoes(ParticipantStatus participant)
        {
            var hatoes = new ParticipantStatusHatoes().PopulateWith(participant);

            hatoes.References.DivisionRef =
                Request.GetBaseUrl().CombineWith(new FetchDivision { Id = participant.DivisionId }.ToGetUrl());

            hatoes.References.DivisionRef =
                Request.GetBaseUrl().CombineWith(new FetchServer { Id = participant.SeasonId }.ToGetUrl());

            return (hatoes);
        }

        public ParticipantResponse Get(FetchParticipant request)
        {
            var jn = new JoinSqlBuilder<Crawler, Participant>();
            jn.Join<Crawler, Participant>(c => c.Id, p => p.CrawlerId,
                c => new { c.UserName },
                p => new { p.CrawlerId, p.LastGame, p.SeasonId, p.DivisionId });
            jn.Join<Participant, Division>(p => p.DivisionId, d => d.Id,
                null,
                d => new { DivisionName = d.Name });
            jn.Where<Participant>(x => x.CrawlerId == request.CrawlerId && request.SeasonId == x.SeasonId);

            var result = Db.Single<ParticipantStatus>(jn.ToSql());

            if (result == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("No match was found."));

            return new ParticipantResponse
            {
                ParticipantStatus = ParitipantStatusHatoes(result)
            };
        }

        public HttpResult Post(CreateParticipant request)
        {
            var participant = new Participant().PopulateWith(request.SanitizeDtoHtml());

            // Check to see if crawler is already part of the leaue
            if (
                Get(new FetchParticipant {CrawlerId = request.CrawlerId, SeasonId = request.SeasonId}).ParticipantStatus !=
                null)
                throw new HttpError(HttpStatusCode.Conflict,
                    new ArgumentException("CrawlerId {0} already belongs to this season. ".Fmt(request.CrawlerId)));

            var crawlerResp = ResolveService<CrawlerService>().Get(new FetchCrawler { Id = request.CrawlerId });
            var seasonResp = ResolveService<SeasonService>().Get(new FetchSeason { Id = request.SeasonId });

            participant.Id = crawlerResp.Crawler.Id;
            participant.DivisionId = crawlerResp.Crawler.DivisionId;

            if (!seasonResp.Season.Active)
                throw new HttpError(HttpStatusCode.BadRequest,
                    new ArgumentException("Season {0} is not currently active.".Fmt(seasonResp.Season.Id)));

            // Validate .rc file for the server
            var serverResp = ResolveService<ServerService>().Get(new FetchServer { Id = crawlerResp.Crawler.ServerId });

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
                        Get(new FetchParticipant { CrawlerId = request.CrawlerId, SeasonId = request.SeasonId })
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
    }
}
