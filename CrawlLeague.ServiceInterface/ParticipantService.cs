using System;
using System.Collections.Generic;
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
    public class ParticipantService : Service
    {
        private readonly CrawlerValidator _validator;

        public ParticipantService(CrawlerValidator validator)
        {
            _validator = validator;
        }
                
        private ParticipantStatusHatoes ParitcipantStatusHatoes(ParticipantStatus participant)
        {
            var hatoes = new ParticipantStatusHatoes().PopulateWith(participant);

            hatoes.References.CrawlerRef =
                Request.GetBaseUrl().CombineWith(new FetchCrawler { Id = participant.CrawlerId }.ToGetUrl());

            hatoes.References.DivisionRef =
                Request.GetBaseUrl().CombineWith(new FetchDivision { Id = participant.DivisionId }.ToGetUrl());

            hatoes.References.SeasonRef =
                Request.GetBaseUrl().CombineWith(new FetchSeason { Id = participant.SeasonId }.ToGetUrl());

            hatoes.References.ServerRef =
                Request.GetBaseUrl().CombineWith(new FetchServer { Id = participant.ServerId }.ToGetUrl());

            return (hatoes);
        }

        public ParticipantsResponse Get(FetchParticipantStatuses request)
        {
            var participants = new List<ParticipantStatusHatoes>();
            
            // Count
            var jn = new JoinSqlBuilder<Crawler, Participant>();
            jn.Join<Crawler, Participant>(c => c.Id, p => p.CrawlerId)
                .Join<Participant, Division>(p => p.DivisionId, d => d.Id)
                .Join<Crawler, Server>(c => c.ServerId, d => d.Id).SelectCount<Participant>(p => p.Id);

            var count = Db.Scalar<long>(jn.ToSql());   
            var results = Db.Select<ParticipantStatus>(GenerateFetchParticipantSql(request));

            results.ForEach(r => participants.Add(ParitcipantStatusHatoes(r)));

            return new ParticipantsResponse
            {
                Standings = participants,
                Paging = new Paging(Request.AbsoluteUri) { Page = request.Page ?? 1, TotalCount = count }
            };
        }

        public ParticipantResponse Get(FetchParticipantStatus request)
        {
            var result = Db.Single<ParticipantStatus>(GenerateFetchParticipantSql(request));

            if (result == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("No match was found."));

            return new ParticipantResponse
            {
                ParticipantStatus = ParitcipantStatusHatoes(result)
            };
        }

        public string GenerateFetchParticipantSql(IParticipantRequest request)
        {
            var fetchRequest = request as FetchParticipantStatus;
            var fetchAllReqeust = request as FetchParticipantStatuses;

            var jn = new JoinSqlBuilder<Participant, Participant>()
                .Join<Crawler, Participant>(c => c.Id, p => p.CrawlerId)
                .Join<Participant, Division>(p => p.DivisionId, d => d.Id)
                .Join<Crawler, Server>(c => c.ServerId, d => d.Id)
                .Select<Crawler>(c => new { c.UserName, c.ServerId })
                .Select<Participant>(p => new { p.CrawlerId, p.LastGame, p.SeasonId, p.DivisionId })
                .Select<Division>(d => new { DivisionName = d.Name })
                .Select<Server>(s => new { ServerName = s.Name, ServerAbbreviation = s.Abbreviation })
                .Where<Participant>(s => s.SeasonId == request.SeasonId);

            jn.OrderByDescending<Participant>(p => p.Score);

            // Restricted by Crawler
            if (fetchRequest != null)
                jn.And<Participant>(p => p.CrawlerId == fetchRequest.CrawlerId);

            // Standings
            if (fetchAllReqeust != null) // Hackaroo pageroo for JoinSqlBuilder.
            {
                jn.And<Division>(d => d.Id == fetchAllReqeust.DivisionId);
                return (jn.ToPagedSql(fetchAllReqeust.Page ?? 1));
            }

            return (jn.ToSql());
        }

        public HttpResult Post(CreateParticipant request)
        {
            var participant = new Participant().PopulateWith(request.SanitizeDtoHtml());

            try
            {
                // Check to see if crawler is already part of the leaue
                Get(new FetchParticipantStatus
                {
                    CrawlerId = request.CrawlerId,
                    SeasonId = request.SeasonId
                });

                throw new HttpError(HttpStatusCode.Conflict,
                    new ArgumentException("CrawlerId {0} already belongs to this season. ".Fmt(request.CrawlerId)));
            }
            catch (HttpError ex)
            {
                if (ex.StatusCode != HttpStatusCode.NotFound)
                    throw;
            }

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
                    new Uri(serverResp.Server.PlayerRcUrl(seasonResp.Season.CrawlVersion, crawlerResp.Crawler.UserName))))
                throw new HttpError(HttpStatusCode.Forbidden,
                    new ArgumentException(
                        "UserName {0} does not have a valid .rc file. ".Fmt(crawlerResp.Crawler.UserName)));

            var newId = Db.Insert(participant, selectIdentity: true);

            return
                new HttpResult(new ParticipantResponse
                {
                    ParticipantStatus =
                        Get(new FetchParticipantStatus
                        {
                            CrawlerId = request.CrawlerId,
                            SeasonId = request.SeasonId
                        })
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
