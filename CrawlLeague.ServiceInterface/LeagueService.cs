using System;
using System.Net;
using CrawlLeague.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;

namespace CrawlLeague.ServiceInterface
{
    public class LeagueService : Service
    {   
        public League Get(League request)
        {
            return Db.SingleById<League>(request.Id);
        }

        public LeaguesResponse Get(Leagues request)
        {
            return new LeaguesResponse
            {
                Leagues = Db.Select<League>() 
            };
        }

        public HttpResult Post(League request)
        {
            Db.Save(request);

            return new HttpResult(Db.SingleById<League>(request.Id))
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                {
                    {HttpHeaders.Location, Request.AbsoluteUri.CombineWith(request.Id)}
                }
            };
        }

        public HttpResult Put(League request)
        {
            var league = Get(request);

            if (league.Start < DateTime.UtcNow)
                throw new Exception("You can't change a league that has already started.");
            Db.Update(request);

            return new HttpResult
            {
                StatusCode = HttpStatusCode.NoContent,
                Headers =
                {
                    {HttpHeaders.Location, Request.AbsoluteUri.CombineWith(request.Id)}
                }
            };
        }
    }
}
