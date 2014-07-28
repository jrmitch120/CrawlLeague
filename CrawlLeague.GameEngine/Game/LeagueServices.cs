using CrawlLeague.ServiceInterface;

namespace CrawlLeague.GameEngine.Game
{
    public class LeagueServices
    {
        public readonly AdminService AdminSvc;
        public readonly ParticipantService ParticipantSvc;

        public LeagueServices(AdminService adminSvc, ParticipantService participantSvc)
        {
            AdminSvc = adminSvc;
            ParticipantSvc = participantSvc;
        }
    }
}
