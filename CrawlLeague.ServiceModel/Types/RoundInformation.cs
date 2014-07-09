using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public class RoundInformation
    {
        public RoundInformation()
        {
            CurrentRound = new Round();
            RoundsToProcess = new SortedList<int,Round>();
        }

        [Description("Total number of rounds")]
        public int TotalRounds { get; set; }

        [Description("What round the season currently is in")]
        public Round CurrentRound { get; private set; }

        [Description("Rounds that need to be processed")]
        public SortedList<int,Round> RoundsToProcess { get; private set; }
    }

    public class Round
    {
        [Description("Number of the round")]
        public int Number { get; set; }

        [Description("When the current round began")]
        public DateTime Start { get; set; }

        [Description("When the current round ends")]
        public DateTime End { get; set; }
    }
}