using System.Collections.Generic;
using CrawlLeague.ServiceModel.Types;
using CrawlLeague.ServiceModel.Util;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    public class SeasonsResponse : IHasResponseStatus
    {
        public List<Season> Seasons { get; set; }
        public Paging Paging { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public SeasonsResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }

    public class SeasonResponse : IHasResponseStatus
    {
        public Season Season { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public SeasonResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }

    public class SeasonStatusResponse : IHasResponseStatus
    {
        public Season Season { get; set; }

        public SeasonStatus Status { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public SeasonStatusResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }

    public class ParticipantResponse : IHasResponseStatus
    {
        public ParticipantStatus ParticipantStatus { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public ParticipantResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}
