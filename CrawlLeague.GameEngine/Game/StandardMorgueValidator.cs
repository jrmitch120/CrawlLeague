using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.GameEngine.Game
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
            if (morgueFile.Contents.Contains(string.Format("version {0}", _crawlVersion)))
                return true;

            return false;
        }
    }
}