using System;
using System.Net;
using CrawlLeague.ServiceInterface.Extensions;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace CrawlLeague.ServiceInterface
{
    public class GameService : Service
    {
        public GameResponse Get(FetchGame request)
        {
            var game = Db.SingleById<Game>(request.Id);

            if (game == null)
                throw new HttpError(HttpStatusCode.NotFound,
                    new ArgumentException("Game {0} does not exist. ".Fmt(request.Id)));

            return new GameResponse { Game = game };
        }

        public HttpResult Post(CreateGame request)
        {
            var game = new Game().PopulateWith(request.SanitizeDtoHtml());
            var newId = Db.Insert(game, selectIdentity: true);

            return new HttpResult(new GameResponse {Game = Get(new FetchGame {Id = (int) newId}).Game})
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                {
                    {HttpHeaders.Location, new FetchGame {Id = (int) newId}.ToGetUrl()}
                }
            };
        }
    }
}
