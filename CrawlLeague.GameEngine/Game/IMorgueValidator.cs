using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.GameEngine.Game
{
    public interface IMorgueValidator
    {
        bool Validate(MorgueFile morgueFile);
    }
}