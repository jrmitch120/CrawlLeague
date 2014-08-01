using System.Text.RegularExpressions;
using CrawlLeague.ServiceModel.Operations;

namespace CrawlLeague.GameEngine
{
    public static class EngineExtensions
    {
        public static void ParseMorgueForStats(this CreateGame game)
        {
            var match = Regex.Match(game.Morgue, "runes: (.+?)(?=You)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            if (match.Success)
            {
                var runes = match.Groups[1].Value.Split(new[] {','});
                game.Score += runes.Length*50;
            }
        }
    }
}

