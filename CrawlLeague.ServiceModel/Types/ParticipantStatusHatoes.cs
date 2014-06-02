using System;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public class ParticipantStatusHatoes
    {
        [Description("References to peripheral resources")]
        private readonly ParticipantRefs _references = new ParticipantRefs();
        public ParticipantRefs References { get { return _references; } }

        [Description("Name of the crawler")]
        public string UserName { get; set; }
        
        [Description("Name of the division that the participant belongs to")]
        public string DivisionName { get; set; }

        [Description("Curent total score for the season")]
        public int Score { get; set; }

        [Description("Number of games played during the season")]
        public int GamesPlayed { get; set; }

        [Description("When was the last game recorded for this participant")]
        public DateTime LastGame { get; set; }
    }

    public class ParticipantRefs
    {
        [Description("Link to the season that the crawler is currently bound to")]
        public string SeasonRef { get; set; }

        [Description("Link to the division that the crawler is slotted into as of the last divison realignment")]
        public string DivisionRef { get; set; }
    }
}