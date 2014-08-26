using CrawlLeague.ServiceInterface;

namespace CrawlLeague.GameEngine.Game
{
    public class LeagueServices
    {
        public readonly AdminService AdminSvc;
        public readonly SeasonService SeasonSvc;
        public readonly GameService GameSvc;

        public LeagueServices(AdminService adminSvc, SeasonService seasonSvc, GameService gameSvc)
        {
            AdminSvc = adminSvc;
            SeasonSvc = seasonSvc;
            GameSvc = gameSvc;
        }
    }
}
