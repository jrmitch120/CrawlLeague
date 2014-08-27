using System;
using System.Data;
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
            var game = Db.LoadSingleById<Game>(request.Id);
            
            if (game == null)
                throw new HttpError(HttpStatusCode.NotFound,
                    new ArgumentException("Game {0} does not exist. ".Fmt(request.Id)));

            return new GameResponse { Game = game };
        }

        public HttpResult Post(CreateGame request)
        {
            var game = new Game().PopulateWith(request.SanitizeDtoHtml());
            int newId;
            using (IDbTransaction trans = Db.OpenTransaction())
            {
                newId = (int) Db.Insert(game, selectIdentity: true);

                var participant = ResolveService<ParticipantService>().Get(new FetchParticipantStatus { CrawlerId = game.CrawlerId, SeasonId = game.SeasonId }).ParticipantStatus;

                // TODO Update particpant counts extension method.

                Db.Update<Participant>(participant);
                
                game.Id = newId;

                Db.SaveAllReferences(game);

                trans.Commit();
            }

            return new HttpResult(new GameResponse {Game = Get(new FetchGame {Id = newId}).Game})
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                {
                    {HttpHeaders.Location, new FetchGame {Id = newId}.ToGetUrl()}
                }
            };
        }
    }
}
