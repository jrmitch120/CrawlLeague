using System;
using CrawlLeague.ServiceModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    [CompositeIndex(true, "CrawlerId", "SeasonId")]
    public class Participant : ParticipantCore, IAudit
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        [Description("Curent total score for the season")]
        public int Score { get; set; }

        [Description("Number of games played during the season")]
        public int GamesPlayed { get; set; }

        [Description("Number of games won during the season")]
        public int WinCount { get; set; }

        [Description("Number of character levels gained")]
        public int TotalCharacaterLevels { get; set; }

        [Description("Number of branches visited during the season")]
        public int BranchesVisited { get; set; }

        [Description("Number of dungeon levels visited during the season")]
        public int LevelsVisited { get; set; }

        [Description("Number of runes collected during the season")]
        public int RuneCount { get; set; }

        [Description("Number of kills made during the season")]
        public int TotalKills { get; set; }

        [Description("When was the last game recorded")]
        public DateTime LastGame { get; set; }

        [Description("When was the last this participant was processed in a game sweep")]
        public DateTime LastProcessed { get; set; }

        [Created]
        public DateTime CreatedDate { get; set; }

        [Modified]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey(typeof(Division))]
        [Description("Id of the division")]
        public int DivisionId { get; set; }

        [Description("References to peripheral resources")]
        [Ignore]
        public ParticipantRefs References { get; set; }
    }

    [Alias("Participant")]
    public abstract class ParticipantCore
    {
        [ForeignKey(typeof(Crawler))]
        [Description("Id of the crawler")]
        public int CrawlerId { get; set; }

        [ForeignKey(typeof(Season))]
        [Description("Id of the season")]
        public int SeasonId { get; set; }
    }
}
