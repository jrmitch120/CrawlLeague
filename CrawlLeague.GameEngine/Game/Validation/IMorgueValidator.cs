using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.GameEngine.Game.Validation
{
    public interface IMorgueValidator
    {
        bool Validate(MorgueFile morgueFile);
    }
}