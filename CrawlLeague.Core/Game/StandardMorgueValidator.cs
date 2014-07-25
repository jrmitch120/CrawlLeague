using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.Core.Game
{
    public class StandardMorgueValidator : IMorgueValidator
    {
        public bool Validate(MorgueFile morgueFile)
        {
            return true;
        }
    }
}