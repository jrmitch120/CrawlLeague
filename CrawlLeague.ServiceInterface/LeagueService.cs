using System;
using System.Collections.Generic;
using System.Net;
using CrawlLeague.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;

namespace CrawlLeague.ServiceInterface
{
    public class LeagueService : Service
    {
        public LeaguesResponse Get(Leagues request)
        {
            return new LeaguesResponse { Leagues = Db.Select<League>() };
        }

        public LeagueResponse Get(LeagueRequest request)
        {
            var league = Db.SingleById<League>(request.Id);

            if (league == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("League {0} does not exist. ".Fmt(request.Id)));

            return new LeagueResponse{League = league};
        }

        public HttpResult Post(League request)
        {
            Db.Insert(request);

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
            var response = Get(new LeagueRequest {Id = request.Id});

            if (response.League.Start < DateTime.UtcNow)
                throw new ArgumentException("You can't change a league that has already started.");

            Db.Update(request);

            return new HttpResult { StatusCode = HttpStatusCode.NoContent};
        }

        public HttpResult Delete(LeagueRequest request)
        {
            int result = Db.DeleteById<League>(request.Id);

            if(result == 0)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("League {0} does not exist. ".Fmt(request.Id)));

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }
    }
}
