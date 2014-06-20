using System.Collections.Generic;
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
            var processingRequests = new List<ProcessingRequest>();
            var seasonResp = ResolveService<SeasonService>().Get(new FetchSeasons {NotFinal = true});

            foreach (Season season in seasonResp.Seasons)
            {
                var processDate = season.Active
                    ? season.RoundInformation().RoundBegins
                    : season.End.AddDays(season.DaysPerRound*-1);

                var jn = new JoinSqlBuilder<Participant, Participant>()
                .Join<Crawler, Participant>(c => c.Id, p => p.CrawlerId)
                .Join<Server, Crawler>(s => s.Id, c => c.ServerId)
                .SelectAll<Participant>()
                .Select<Server>(s => new { s.MorgueUrl})
                .Select<Crawler>(c => new { c.UserName })
                .Where<Participant>(p => p.LastGame < processDate);

                var results = Db.Select<TestMe>(jn.ToSql());

                results.ForEach(r =>
                {
                    var processRequest = new ProcessingRequest
                    {
                        Participant = new Participant().PopulateWith(r),
                        MorgueUrl = new Server {MorgueUrl = r.MorgueUrl}.PlayerMorgueUrl(r.UserName)
                    };

                    processingRequests.Add(processRequest);
                });
            }

            return new ProcessRequestsResponse { ProcessRequests = processingRequests };
        }

        public class TestMe : Participant
        {
            public string MorgueUrl { get; set; }
            public string UserName { get; set; }
        }
    }
}
