using System;

namespace CrawlLeague.Core.Scrapping
{
    public class ScraperResponse
    {
        public Uri Uri { get; set; }
        public string Body { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
    }
}