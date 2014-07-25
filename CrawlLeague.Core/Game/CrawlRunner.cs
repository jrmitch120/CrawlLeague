using System;
using System.Collections.Generic;
using CrawlLeague.Core.Scrapping;
using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.Core.Game
{
    public class CrawlWebRunner : ICrawlRunner
    {
        private readonly IScraper _scraper;

        public CrawlWebRunner(IScraper scraper)
        {
            _scraper = scraper;
        }

        public IList<MorgueFile> GetValidMorgues(RoundProcessRequest roundRequest, IMorgueValidator validator)
        {
            IDictionary<GameFetchRequest, ScraperResponse> indexResponses =
                _scraper.Scrape(IndexRequests(roundRequest.GameFetchRequests));

            IDictionary<GameFetchRequest, ScraperResponse> morgueResponses =
                _scraper.Scrape(MorgueFileRequests(indexResponses));

            // TODO Parse out valid morgues and return

            return new List<MorgueFile>();
        }

        private IDictionary<GameFetchRequest, ScraperRequest> IndexRequests(IEnumerable<GameFetchRequest> gameRequests)
        {
            var indexRequests = new Dictionary<GameFetchRequest, ScraperRequest>();

            foreach (GameFetchRequest gameRequest in gameRequests)
                indexRequests.Add(gameRequest, new ScraperRequest {Uri = new System.Uri(gameRequest.MorgueUrl)});

            return indexRequests;
        }

        private IDictionary<GameFetchRequest, ScraperRequest> MorgueFileRequests(IDictionary<GameFetchRequest, ScraperResponse> indexResponses)
        {
            foreach (GameFetchRequest key in indexResponses.Keys)
            {
                //indexResponses[key].
            }

            return null; // TODO
        }

        private List<MorgueFile> GetValidMorgues(DateTime start, DateTime end,
            IDictionary<GameFetchRequest, ScraperResponse> morgueResponses)
        {
            // 1: url (relative), 2:filename, 3: timestamp, 4: size
            //<tr>[\s]*<td[^>]*>.*?</td>[\s]*<td[^>]*><a href="([^.]*.txt)">*([^<]*)</a></td>[\s]*<td[^>]*>(.*?)</td>[\s]*<td[^>]*>(.*?)</td></tr>

            return null; // TODO
        }
    }
}
