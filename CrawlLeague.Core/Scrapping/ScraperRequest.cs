using System;

namespace CrawlLeague.Core.Scrapping
{
    public class ScraperRequest
    {
        public Uri Uri { get; set; }
        public Action<ScraperResponse> OnCompleted { get; set; }
        public ScrapperOptions Options { get; set; }
    }
}