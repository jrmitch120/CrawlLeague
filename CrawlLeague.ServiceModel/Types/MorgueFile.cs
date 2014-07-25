using System;

namespace CrawlLeague.ServiceModel.Types
{
    public class MorgueFile
    {
        public int CrawlerId { get; set; }
        public int ParticipantId { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public DateTime LastModified { get; set; }
        public string Contents { get; set; }
    }
}
