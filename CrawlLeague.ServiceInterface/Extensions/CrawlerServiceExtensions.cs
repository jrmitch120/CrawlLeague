using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.Web;

namespace CrawlLeague.ServiceInterface.Extensions
{
    public static class CrawlerServiceExtensions
    {
        public static Crawler MapReferences(this Crawler crawler, IRequest request)
        {
            crawler.References = new CrawlerRefs
            {
                DivisionRef = request.GetBaseUrl().CombineWith(new FetchDivision { Id = crawler.DivisionId }.ToGetUrl()),
                ServerRef = request.GetBaseUrl().CombineWith(new FetchServer { Id = crawler.ServerId }.ToGetUrl())
            };

            return crawler;
        }
    }
}
