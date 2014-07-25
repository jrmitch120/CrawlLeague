using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.Core.Game
{
    public interface IMorgueValidator
    {
        bool Validate(MorgueFile morgueFile);
    }
}