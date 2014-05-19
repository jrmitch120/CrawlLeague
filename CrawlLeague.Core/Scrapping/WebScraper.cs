using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrawlLeague.Core.Scrapping
{
    public class WebScraper : IScraper
    {
        private readonly IScraperRequestRunner _runner;

        public WebScraper(IScraperRequestRunner runner)
        {
            _runner = runner;
        }

        public IDictionary<TKey, ScraperResponse> Scrape<TKey>(IDictionary<TKey, ScraperRequest> request)
        {
            var responses = new ConcurrentDictionary<TKey, ScraperResponse>();
            
            Parallel.ForEach(request, current =>
            {
                ScraperRequest req = current.Value;

                try
                {                   
                    var response = new ScraperResponse
                    {
                        Uri = req.Uri,
                        Success = true,
                        Body = _runner.Fetch(req.Uri, req.Options)
                    };

                    responses.TryAdd(current.Key, response);

                    if(req.OnCompleted != null)
                        req.OnCompleted.Invoke(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = new ScraperResponse {Uri = req.Uri, Success = false, ErrorMessage = ex.Message};
                    responses.AddOrUpdate(current.Key, errorResponse, (k, v) => errorResponse);
                }
            });
            
            return (responses);
        }

        public ScraperResponse Scrape(ScraperRequest request)
        {
            var req = new Dictionary<string, ScraperRequest> {{"1", request}};
            var response = Scrape(req);

            return response.ToArray().First().Value;
        }
    }
}
