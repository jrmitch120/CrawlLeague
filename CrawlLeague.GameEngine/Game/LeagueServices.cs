using CrawlLeague.ServiceInterface;

namespace CrawlLeague.GameEngine.Game
{
    public class LeagueServices
    {
        public readonly AdminService AdminSvc;
        public readonly GameService GameSvc;

        public LeagueServices(AdminService adminSvc, GameService gameSvc)
        {
            AdminSvc = adminSvc;
            GameSvc = gameSvc;
        }
    }
}
