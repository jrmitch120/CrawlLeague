using System.Collections.Generic;
using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.Core.Game
{
    public class GameEngine
    {
        private readonly GameRunner _runner;
        private readonly GameProcessor _processor;

        public bool Running { get; private set; }

        public GameEngine(GameRunner runner, GameProcessor processor)
        {
            _runner = runner;
            _processor = processor;
        }

        public void Run(IEnumerable<Participant> participants, Season season)
        {
            Running = true;

            foreach (Participant participant in participants)
            {
                //season.RoundInformation
            }

            Running = false;
        }
    }
}
