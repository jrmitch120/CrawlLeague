using System;
using System.Collections.Generic;
using System.Threading;
using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.Core.Game
{
    public class GameEngine
    {
        private readonly CrawlRunner _runner;
        private readonly CrawlProcessor _processor;

        private volatile bool _running;
        private Timer _timer;
        private readonly TimeSpan _runInterval = TimeSpan.FromMinutes(1);

        public GameEngine(CrawlRunner runner, CrawlProcessor processor)
        {
            _runner = runner;
            _processor = processor;
        }

        public void Start()
        {
            _timer = new Timer(_ => Run(), null, TimeSpan.Zero, _runInterval);
        }

        private void Run()
        {
            if (_running)
                return;

            try
            {
                Thread.Sleep(10000);
            }
            // TODO: catch (Exception ex) { }
            finally
            {
                _running = false;    
            }
            
        }
    }
}
