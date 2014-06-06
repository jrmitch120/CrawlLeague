using System;

namespace CrawlLeague.ServiceModel.Types
{
    // Select DTO for complex join
    public class ParticipantStatus
    {
        public int CrawlerId { get; set; }

        public int SeasonId { get; set; }

        public int DivisionId { get; set; }

        public string DivisionName { get; set; }

        public int ServerId { get; set; }

        public string ServerName { get; set; }

        public string ServerAbbreviation { get; set; }

        public string UserName { get; set; }

        public int Score { get; set; }

        public int GamesPlayed { get; set; }

        public int WinCount { get; set; }

        public int TotalCharacaterLevels { get; set; }

        public int BranchesVisited { get; set; }

        public int LevelsVisited { get; set; }

        public int RuneCount { get; set; }

        public int TotalKills { get; set; }

        public DateTime LastGame { get; set; }
    }
}