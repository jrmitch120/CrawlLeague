using CrawlLeague.ServiceInterface.RequestFilters;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace CrawlLeague.ServiceInterface
{
    [ApiKey]
    public class AdminService : Service
    {
        public ProcessRequestsResponse Get(FetchProcessRequests request)
        {
            var processingRequest = new ProcessingRequest();

            var seasonResp = ResolveService<SeasonService>().Get(new FetchSeasons { NotFinal = true });

            foreach (Season season in seasonResp.Seasons)
            {
                var seasonRequest = new SeasonProcessRequest {SeasonId = season.Id};

                foreach (var round in season.RoundInformation.RoundsToProcess.Values)
                {
                    var roundRequest = new RoundProcessRequest {Round = round};

                    var finishedCrawlers =
                        Db.SqlColumn<int>(new JoinSqlBuilder<Game, Game>()
                            .Join<Participant, Game>(p => p.Id, g => g.ParticipantId)
                            .Select<Participant>(p => p.CrawlerId)
                            .Where<Game>(
                                x =>
                                    x.CompletedDate >= round.Start &&
                                    x.CompletedDate <= round.End)
                            .And<Participant>(p => p.SeasonId == season.Id).ToSql());

                    var gamesNeeded = Db.Select<TestMe>(new JoinSqlBuilder<Participant, Participant>()
                        .Join<Crawler, Participant>(c => c.Id, p => p.CrawlerId)
                        .Join<Participant, Season>(p => p.SeasonId, s => s.Id)
                        .Join<Server, Crawler>(s => s.Id, c => c.ServerId)
                        .Select<Participant>(p => new {ParticipantId = p.Id, p.CrawlerId})
                        .Select<Server>(s => new {s.MorgueUrl, s.UtcOffset})
                        .Select<Crawler>(c => new {c.UserName})
                        .Select<Season>(s => new {s.CrawlVersion})
                        .Where<Participant>(p => p.SeasonId == season.Id) // Specific season
                        .And<Participant>(p => !Sql.In(p.CrawlerId, finishedCrawlers)) // Not played a game
                        .And<Crawler>(c => !c.Banned).ToSql());

                    gamesNeeded.ForEach(r => roundRequest.GameFetchRequests.Add(new GameFetchRequest
                    {
                        CrawlerId = r.CrawlerId,
                        ParticipantId = r.ParticipantId,
                        MorgueUrl = new Server {MorgueUrl = r.MorgueUrl}.PlayerMorgueUrl(r.CrawlVersion, r.UserName),
                        UtcOffset = r.UtcOffset
                    }));

                    seasonRequest.RoundProcessRequests.Add(roundRequest);
                }

                processingRequest.SeasonProcessRequests.Add(seasonRequest);
            }

            return new ProcessRequestsResponse {ProcessRequest = processingRequest};
        }

        public class TestMe
        {
            public string UserName { get; set; }
            public int CrawlerId { get; set; }
            public int ParticipantId { get; set; }
            public string CrawlVersion { get; set; }
            public string MorgueUrl { get; set; }
            public int UtcOffset { get; set; }
        }
    }
}
