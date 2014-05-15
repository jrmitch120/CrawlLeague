using System;
using CrawlLeague.Core.Scrapping;

namespace CrawlLeague.Core.Verification
{
    public class CrawlerValidator
    {
        private readonly IScraper _scraper;

        public CrawlerValidator(IScraper scraper)
        {
            _scraper = scraper;
        }

        public bool ValidateRcInit(Uri rcfile)
        {
            var result = _scraper.Scrape(new ScraperRequest {Uri = rcfile});

            if (result.Success)
                return (result.Body.IndexOf("#CrawlLeague Enabled",StringComparison.CurrentCultureIgnoreCase) >= 0);

            return (false);
        }

        public bool ValidateRcTransfer()
        {
            throw new NotImplementedException();
        }
    }
}
