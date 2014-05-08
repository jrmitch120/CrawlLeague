using ServiceStack;
using ServiceStack.Web;

namespace CrawlLeague.ServiceInterface.RequestFilters
{
    public class ApiKeyAttribute : RequestFilterAttribute
    {
        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            var appConfig = req.TryResolve<AppConfig>();
            var apiKey = req.Headers["X-ApiKey"] ?? req.QueryString["api_key"];
            if (apiKey == null || !appConfig.ReadWriteApiKeys.Contains(apiKey))
            {
                throw HttpError.Unauthorized("Unauthorized.  Valid X-ApiKey header required.");
            } 
        }
    }
}