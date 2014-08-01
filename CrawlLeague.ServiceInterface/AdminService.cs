using System;
using CrawlLeague.ServiceInterface.RequestFilters;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace CrawlLeague.ServiceInterface
{
    [ApiKey]
    public class AdminService : Service
    {
        public ProcessRequestResponse Get(FetchProcessRequest request)
        {
            var processingRequest = new ProcessingRequest();

            var seasonResp = ResolveService<SeasonService>().Get(new FetchSeasons { NotFinal = true });

            foreach (Season season in seasonResp.Seasons)
            {
                var seasonRequest = new SeasonProcessRequest {SeasonId = season.Id, CrawlVersion = season.CrawlVersion};

                foreach (var round in season.RoundInformation.RoundsToProcess.Values)
                {
                    var roundRequest = new RoundProcessRequest {Round = round};
                    
                    // Crawlers that have already played a game
                    var finishedCrawlers =
                        Db.SqlColumn<int>(
                            Db.From<Game>()
                                .Select(g => g.CrawlerId)
                                .Where(g => g.CompletedDate >= round.Start)
                                .And(g => g.CompletedDate <= round.End)
                                .And(g => g.SeasonId == season.Id));

                    // Crawlers that need a game (select crawlers excluding the completed crawlers)
                    var gamesNeeded = Db.Select<TestMe>(new JoinSqlBuilder<Participant, Participant>()
                        .Join<Crawler, Participant>(c => c.Id, p => p.CrawlerId)
                        .Join<Participant, Season>(p => p.SeasonId, s => s.Id)
                        .Join<Server, Crawler>(s => s.Id, c => c.ServerId)
                        .Select<Participant>(p => new {ParticipantId = p.Id, p.CrawlerId})
                        .Select<Server>(s => new {s.MorgueUrl, s.UtcOffset})
                        .Select<Crawler>(c => new {c.UserName})
                        .Select<Season>(s => new {s.CrawlVersion, SeasonStart = s.Start})
                        .Where<Participant>(p => p.SeasonId == season.Id) // Specific season
                        .And<Participant>(p => !Sql.In(p.CrawlerId, finishedCrawlers)) // Not played a game
                        .And<Crawler>(c => !c.Banned).ToSql());

                    // Create lookup requests
                    gamesNeeded.ForEach(r => roundRequest.GameFetchRequests.Add(new GameFetchRequest
                    {
                        CrawlerId = r.CrawlerId,
                        ParticipantId = r.ParticipantId,
                        MorgueUrl = new Server {MorgueUrl = r.MorgueUrl}.PlayerMorgueUrl(r.CrawlVersion, r.UserName),
                        MorguesSince = r.LastProcessed == DateTime.MinValue ? r.SeasonStart : r.LastProcessed,
                        UtcOffset = r.UtcOffset,
                    }));

                    seasonRequest.RoundProcessRequests.Add(roundRequest);
                }

                processingRequest.SeasonProcessRequests.Add(seasonRequest);
            }

            return new ProcessRequestResponse {ProcessRequest = processingRequest};
        }

        public class TestMe
        {
            public string UserName { get; set; }
            public int CrawlerId { get; set; }
            public int ParticipantId { get; set; }
            public string CrawlVersion { get; set; }
            public string MorgueUrl { get; set; }
            public DateTime SeasonStart { get; set; }
            public DateTime LastProcessed { get; set; }
            public int UtcOffset { get; set; }
        }
    }
}
