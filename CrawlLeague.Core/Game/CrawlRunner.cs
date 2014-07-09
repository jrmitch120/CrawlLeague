using CrawlLeague.Core.Scrapping;

namespace CrawlLeague.Core.Game
{
    public class CrawlRunner
    {
        private readonly IScraper _scraper;

        public CrawlRunner(IScraper scraper)
        {
            _scraper = scraper;
        }
    }
}
