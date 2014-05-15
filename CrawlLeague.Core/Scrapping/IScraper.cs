using System.Collections.Generic;

namespace CrawlLeague.Core.Scrapping
{
    public interface IScraper
    {
        IDictionary<TKey, ScraperResponse> Scrape<TKey>(IDictionary<TKey, ScraperRequest> request);
        ScraperResponse Scrape(ScraperRequest request);
    }
}