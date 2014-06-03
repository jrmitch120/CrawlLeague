using CrawlLeague.Core.Scrapping;

namespace CrawlLeague.Core.Game
{
    public class GameRunner
    {
        private readonly IScraper _scraper;

        public GameRunner(IScraper scraper)
        {
            _scraper = scraper;
        }
    }
}
