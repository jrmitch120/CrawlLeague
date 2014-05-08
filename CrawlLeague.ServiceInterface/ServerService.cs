using System;
using System.Net;
using CrawlLeague.ServiceModel;
using CrawlLeague.ServiceModel.Operations;
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
                Servers = Db.Select<Server>(q => q.Limit(skip: (page - 1)*Paging.PageSize, rows: Paging.PageSize)),
                Paging = new Paging {Page = page, TotalCount = Convert.ToInt32(Db.Count<Season>())}
            };
        }

        public ServerResponse Get(FetchServer request)
        {
            var server = Db.SingleById<Server>(request.Id);

            if (server == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Server {0} does not exist. ".Fmt(request.Id)));

            return new ServerResponse { Server = server };
        }
    }
}
