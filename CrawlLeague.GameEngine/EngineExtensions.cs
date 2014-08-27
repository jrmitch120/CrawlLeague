using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.GameEngine
{
    public static class EngineExtensions
    {
        public static void ParseMorgueForStats(this CreateGame game)
        {
            game.Runes = new List<Rune>();

            var match = Regex.Match(game.Morgue, "runes: (.+?)(?=You)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            if (match.Success)
            {
                foreach (var runeName in match.Groups[1].Value.Split(new[] {','}))
                {
                    game.Score += 50;  // 50 pts a rune.

                    game.Runes.Add(new Rune
                    {
                        Type = (RuneType)Enum.Parse(typeof(RuneType), runeName, true)
                    });
                }
            }
        }
    }
}

