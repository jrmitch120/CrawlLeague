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

        public string PlayerMorgueUrl(string crawlVersion,string playerName)
        {
            return FormatUrl(MorgueUrl, crawlVersion, playerName);
        }

        public string PlayerRcUrl(string playerName)
        {
            return (PlayerRcUrl("git", playerName));
        }

        public string PlayerRcUrl(string crawlVersion, string playerName)
        {
            return FormatUrl(RcUrl, crawlVersion, playerName);
        }

        private string FormatUrl(string url, string crawlVersion = null, string playerName = null)
        {
            if (crawlVersion != null)
                url = url.Replace("{crawlVersion}", crawlVersion);

            if (playerName != null)
                url = url.Replace("{userName}", playerName);

            return url;
        }
    }
}
