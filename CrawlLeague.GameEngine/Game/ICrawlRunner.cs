using System.Collections.Generic;
using CrawlLeague.GameEngine.Game.Validation;
using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.GameEngine.Game
{
    public interface ICrawlRunner
    {
        IList<MorgueFile> GetValidMorgues(RoundProcessRequest roundRequest, IMorgueValidator validator);
    }
}