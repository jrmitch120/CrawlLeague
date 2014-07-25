using System.Collections.Generic;
using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.Core.Game
{
    public interface ICrawlRunner
    {
        IList<MorgueFile> GetValidMorgues(RoundProcessRequest roundRequest, IMorgueValidator validator);
    }
}