using System;
using CrawlLeague.Core.Util;
using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.GameEngine.Game.Validation
{
    public class StandardMorgueValidator : IMorgueValidator
    {
        private readonly string _crawlVersion;

        public StandardMorgueValidator(string crawlVersion)
        {
            _crawlVersion = crawlVersion;
        }

        public bool Validate(MorgueFile morgueFile)
        {
            // Morgue files can come from different game types ex) sprint.  Look for this exact phrase.
            return
                morgueFile.Contents.Contains(
                    string.Format("Dungeon Crawl Stone Soup version {0}", _crawlVersion),
                    StringComparison.OrdinalIgnoreCase);
        }
    }
}