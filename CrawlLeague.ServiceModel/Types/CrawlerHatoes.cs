using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public class CrawlerHatoes
    {
        [AutoIncrement]
        public int Id { get; set; }

        [Index(Unique = true)]
        [Description("Crawler's user name.")]
        public string UserName { get; set; }

        [Description("Link to the server that the crawler is currently bound to")]
        public string ServerRef { get; set; }

        [Description("Link to the division that the crawler is slotted into as of the last divison realignment")]
        public string DivisionRef { get; set; }

        [Description("Active or inactive crawler")]
        public bool Active { get; set; }

        [Description("Has the crawler been banned")]
        public bool Banned { get; set; }

        [Description("Reason for a banning")]
        public string BanReason { get; set; }
    }
}
