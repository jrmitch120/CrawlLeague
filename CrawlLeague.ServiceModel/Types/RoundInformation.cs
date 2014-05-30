using System;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public class RoundInformation
    {
        [Description("Total number of rounds")]
        public int TotalRounds { get; set; }
        
        [Description("What round the season currently is in")]
        public int CurrentRound { get; set; }

        [Description("When the current round began")]
        public DateTime RoundBegins { get; set; }

        [Description("When the current round ends")]
        public DateTime RoundEnds { get; set; }
    }
}