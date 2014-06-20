using System;
using CrawlLeague.ServiceModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public class Server : IAudit
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        [Description("Name of the crawl server")]
        public string Name { get; set; }

        [Index(Unique=true)]
        [Description("Commonly accepted server shorthand")]
        public string Abbreviation { get; set; }

        [Description("Url of the server")]
        public string Url { get; set; }

        [Description("Url of the server's .rc files")]
        public string RcUrl { get; set; }

        [Description("Url of the server's morque list")]
        public string MorgueUrl { get; set; }

        [Description("Designates an active or inactive server")]
        public bool Active { get; set; }

        [Created]
        public DateTime CreatedDate { get; set; }

        [Modified]
        public DateTime ModifiedDate { get; set; }

        public string PlayerMorgueUrl(string playerName)
        {
            return String.Format(MorgueUrl, playerName);
        }

        public string PlayerRcUrl(string playerName)
        {
            return (PlayerRcUrl("git", playerName));
        }

        public string PlayerRcUrl(string version, string playerName)
        {
            return (String.Format(RcUrl, "crawl-" + version, playerName));
        }
    }
}
