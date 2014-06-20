using System;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public class Season : SeasonCore, IAudit
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        [Description("Is the season currently active")]
        [Ignore]
        public bool Active
        {
            get { return (DateTime.UtcNow > Start && DateTime.Now < End); }
        }

        [Description("Has the season been post processed")]
        public bool Finalized { get; set; }

        [Description("When the season was created (UTC)")]
        public DateTime CreatedDate { get; set; }

        [Description("Last time the season was modified (UTC)")]
        public DateTime ModifiedDate { get; set; }

        public RoundInformation RoundInformation()
        {
            var round = new RoundInformation { TotalRounds = (End - Start).Days / DaysPerRound };

            if ((End - Start).Days % DaysPerRound != 0)
                round.TotalRounds++;

            if (DateTime.UtcNow < Start)
            {
                round.CurrentRound = 1;
                round.RoundBegins = Start;
                round.RoundEnds = Start.AddDays(DaysPerRound);
            }
            if (DateTime.UtcNow > End)
            {
                round.CurrentRound = round.TotalRounds;
                round.RoundBegins = End.AddDays(DaysPerRound * -1);
                round.RoundEnds = End;
            }

            round.CurrentRound = (DateTime.UtcNow - Start).Days / DaysPerRound + 1;
            round.RoundBegins = Start.AddDays((round.CurrentRound - 1) * DaysPerRound);

            round.RoundEnds = round.RoundBegins.AddDays(DaysPerRound) > End
                ? End
                : round.RoundBegins.AddDays(DaysPerRound);

            return (round);
        }
    }

    public abstract class SeasonCore 
    {
        [Description("Name of the season")]
        public string Name { get; set; }
        
        [Description("Short description of the season")]
        public string Description { get; set; }

        [Description("Version of Dungeon Crawl that is being used")]
        public string CrawlVersion { get; set; }

        [Description("How many days a given round lasts before advancing")]
        public int DaysPerRound { get; set; }

        [Description("Start of the season (UTC)")]
        public DateTime Start { get; set; }

        [Description("End of the season (UTC)")]
        public DateTime End { get; set; }
    }
}
