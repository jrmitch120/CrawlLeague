using CrawlLeague.ServiceModel.Operations;

namespace CrawlLeague.GameEngine
{
    public static class EngineExtensions
    {
        public static void ParseMorgueForStats(this CreateGame game)
        {
            game.Score = 100;
        }
    }
}
