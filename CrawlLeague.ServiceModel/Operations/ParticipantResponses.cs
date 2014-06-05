using System.Collections.Generic;
using CrawlLeague.ServiceModel.Types;
using CrawlLeague.ServiceModel.Util;
using ServiceStack;

namespace CrawlLeague.ServiceModel.Operations
{
    public class ParticipantsResponse : IHasResponseStatus
    {
        public List<ParticipantStatusHatoes> Standings { get; set; }
        public Paging Paging { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public ParticipantsResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }

    public class ParticipantResponse : IHasResponseStatus
    {
        public ParticipantStatusHatoes ParticipantStatus { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public ParticipantResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}
