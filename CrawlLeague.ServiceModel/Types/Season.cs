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

        [Description("Last time the season was processed for new player data (UTC)")]
        public DateTime LastProcessed { get; set; }

        [Ignore]
        [Description("Information for the season's rounds")]
        public RoundInformation RoundInformation
        {
            get
            {
                var info = new RoundInformation {TotalRounds = (End - Start).Days/DaysPerRound};

                if ((End - Start).Days%DaysPerRound != 0)
                    info.TotalRounds++;

                if (DateTime.UtcNow < Start)
                {
                    info.CurrentRound.Number = 1;
                    info.CurrentRound.Start = Start;
                    info.CurrentRound.End = Start.AddDays(DaysPerRound);
                }
                if (DateTime.UtcNow > End)
                {
                    info.CurrentRound.Number = info.TotalRounds;
                    info.CurrentRound.Start = End.AddDays(DaysPerRound * -1);
                    info.CurrentRound.End = End;
                }

                info.CurrentRound.Number = (DateTime.UtcNow - Start).Days/DaysPerRound + 1;
                info.CurrentRound.Start = Start.AddDays((info.CurrentRound.Number - 1) * DaysPerRound);

                info.CurrentRound.End = info.CurrentRound.Start.AddDays(DaysPerRound) > End
                    ? End
                    : info.CurrentRound.Start.AddDays(DaysPerRound);

                
                for (int i = 0; i < info.CurrentRound.Number ; i++)
                {
                    var roundsBack = i * -1;

                    var processRound = new Round
                    {
                        Start = info.CurrentRound.Start.AddDays(roundsBack * DaysPerRound),
                        End = info.CurrentRound.End.AddDays(roundsBack * DaysPerRound),
                        Number = info.CurrentRound.Number + (roundsBack)
                    };

                    if (LastProcessed < processRound.End)
                        info.RoundsToProcess.Add(processRound.Number, processRound);
                    else
                        break;
                }

                return (info);
            }
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
