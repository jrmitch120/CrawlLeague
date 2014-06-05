using System;
using System.Net;
using CrawlLeague.ServiceInterface.Extensions;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using CrawlLeague.ServiceModel.Util;

using ServiceStack;
using ServiceStack.OrmLite;

namespace CrawlLeague.ServiceInterface
{
    public class ServerService : Service
    {
        public ServersResponse Get(FetchServers request)
        {
            int page = request.Page ?? 1;
            return new ServersResponse
            {
                Servers = Db.Select<Server>(q => q.PageTo(page)),
                Paging = new Paging(Request.AbsoluteUri) { Page = page, TotalCount = Db.Count<Server>() }
            };
        }

        public ServerResponse Get(FetchServer request)
        {
            var server = Db.SingleById<Server>(request.Id);

            if (server == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Server {0} does not exist. ".Fmt(request.Id)));

            return new ServerResponse { Server = server };
        }

        public HttpResult Post(CreateServer request)
        {
            
            var newId = Db.Insert((Server)request.SanitizeDtoHtml(), selectIdentity: true);

            return new HttpResult(new ServerResponse { Server = Db.SingleById<Server>(newId) })
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                {
                    {HttpHeaders.Location, Request.AbsoluteUri.CombineWith(request.Id)}
                }
            };
        }

        public HttpResult Put(UpdateServer request)
        {
            int result = Db.Update((Server)request.SanitizeDtoHtml());

            if (result == 0)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Server {0} does not exist. ".Fmt(request.Id)));

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }

        public HttpResult Delete(DeleteServer request)
        {
            int result = Db.DeleteById<Server>(request.Id);

            if (result == 0)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Server {0} does not exist. ".Fmt(request.Id)));

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }
    }
}
