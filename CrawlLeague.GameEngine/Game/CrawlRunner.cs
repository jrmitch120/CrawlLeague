using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CrawlLeague.Core.Scrapping;
using CrawlLeague.GameEngine.Game.Validation;
using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.GameEngine.Game
{
    public class CrawlWebRunner : ICrawlRunner
    {
        private readonly IScraper _scraper;

        // 1: url (relative), 2:filename, 3: timestamp, 4: size
        private const string MorgueIndexRegex =
            @"<tr>[\s]*<td[^>]*>.*?</td>[\s]*<td[^>]*><a href=""(morgue-[^.]*.txt)"">*([^<]*)</a></td>[\s]*<td[^>]*>(.*?)</td>[\s]*<td[^>]*>(.*?)</td>";

        public CrawlWebRunner(IScraper scraper)
        {
            _scraper = scraper;
        }

        public IList<MorgueFile> GetValidMorgues(RoundProcessRequest roundRequest, IMorgueValidator validator)
        {
            IDictionary<GameFetchRequest, ScraperResponse> moregueIndexes =
                GetMorgueIndexes(roundRequest.GameFetchRequests);

            List<MorgueFile> morgueFiles =
                GetMorgueFiles(moregueIndexes, roundRequest.Round);

            var validMorgueFiles = new List<MorgueFile>();

            foreach (GameFetchRequest gameRequest in roundRequest.GameFetchRequests)
            {
                var morgue =
                    GetFirstValidMorgueFile(morgueFiles.Where(m => m.ParticipantId == gameRequest.ParticipantId), validator);

                if (morgue != null)
                    validMorgueFiles.Add(morgue);
            }

            return validMorgueFiles;
        }

        private IDictionary<GameFetchRequest, ScraperResponse> GetMorgueIndexes(IEnumerable<GameFetchRequest> gameRequests)
        {
            var indexRequests = new Dictionary<GameFetchRequest, ScraperRequest>();

            foreach (GameFetchRequest gameRequest in gameRequests)
                indexRequests.Add(gameRequest, new ScraperRequest {Uri = new Uri(gameRequest.MorgueUrl)});

            return _scraper.Scrape(indexRequests);
        }

        private List<MorgueFile> GetMorgueFiles(IDictionary<GameFetchRequest, ScraperResponse> indexResponses, Round round)
        {
            var morgueFileRequests = new Dictionary<MorgueFile, ScraperRequest>();
            var morgueFiles = new List<MorgueFile>();

            foreach (GameFetchRequest gameRequest in indexResponses.Keys)
            {
                if (indexResponses[gameRequest].Success)
                {
                    var games = Regex.Matches(indexResponses[gameRequest].Body, MorgueIndexRegex);

                    foreach (Match game in games)
                    {
                        var morgue = new MorgueFile
                        {
                            CrawlerId = gameRequest.CrawlerId,
                            ParticipantId = gameRequest.ParticipantId,
                            Url = "http://" + indexResponses[gameRequest].Uri.Host +
                                  indexResponses[gameRequest].Uri.AbsolutePath + 
                                  game.Groups[1].ToString().Trim(),
                            FileName = game.Groups[2].ToString().Trim(),
                            LastModified = DateTime.Parse(game.Groups[3].ToString()).AddHours(gameRequest.UtcOffset),
                        };

                        // Don't look at morgues since the last process date.
                        if (morgue.LastModified < gameRequest.MorguesSince.AddHours(gameRequest.UtcOffset))
                            break;

                        // Games should be sorted by date descending.  In order to help speed up processing, quit
                        // if we've past the end date
                        if (morgue.LastModified < round.Start)
                            break;

                        // Good morgue possibility.  Add it to list 
                        if (morgue.LastModified <= round.End)
                            morgueFileRequests.Add(morgue, new ScraperRequest {Uri = new Uri(morgue.Url)});
                    }
                }
            }

            foreach (var morgueResponse in _scraper.Scrape(morgueFileRequests))
            {
                if (morgueResponse.Value.Success)
                {
                    morgueResponse.Key.Contents = morgueResponse.Value.Body;
                    morgueFiles.Add(morgueResponse.Key);
                }
            }

            return morgueFiles;
        }

        private MorgueFile GetFirstValidMorgueFile(IEnumerable<MorgueFile> files, IMorgueValidator validator)
        {
            return files.OrderBy(m => m.LastModified).FirstOrDefault(validator.Validate);
        }
    }
}
