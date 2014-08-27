using System.Collections.Generic;
using CrawlLeague.ServiceModel.Types;
using CrawlLeague.ServiceModel.Util;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    public class ParticipantResponse : IHasResponseStatus
    {
        public Participant Participant { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public ParticipantResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }

    public class ParticipantsStatusResponse : IHasResponseStatus
    {
        public List<ParticipantStatus> Standings { get; set; }
        public Paging Paging { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public ParticipantsStatusResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }

    public class ParticipantStatusResponse : IHasResponseStatus
    {
        public ParticipantStatus ParticipantStatus { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public ParticipantStatusResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}
