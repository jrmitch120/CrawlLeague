using System;
using System.Threading;
using CrawlLeague.GameEngine.Game.Validation;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;

namespace CrawlLeague.GameEngine.Game
{
    public class LeagueEngine
    {
        private readonly ICrawlRunner _runner;
        private readonly LeagueServices _services;

        private volatile bool _running;
        private Timer _timer;
        private readonly TimeSpan _runInterval = TimeSpan.FromMinutes(1);

        public LeagueEngine(ICrawlRunner runner, LeagueServices services)
        {
            _runner = runner;
            _services = services;
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
                var response = _services.AdminSvc.Get(new FetchProcessRequest());

                foreach (SeasonProcessRequest seasonRequest in response.ProcessRequest.SeasonProcessRequests)
                {
                    foreach (var roundRequest in seasonRequest.RoundProcessRequests)
                    {
                        var morgues = _runner.GetValidMorgues(roundRequest,
                            new StandardMorgueValidator(seasonRequest.CrawlVersion));

                        foreach (MorgueFile morgue in morgues)
                        {
                            var game = new CreateGame
                            {
                                SeasonId = seasonRequest.SeasonId,
                                CrawlerId = morgue.CrawlerId,
                                CompletedDate = morgue.LastModified,
                                Morgue = morgue.Contents
                            };

                            game.ParseMorgueForStats();

                            _services.GameSvc.Post(game);
                        }
                    }
                }

                //Thread.Sleep(10000);
            }
            // TODO: catch (Exception ex) { }
            finally
            {
                _running = false;    
            }
            
        }
    }
}
