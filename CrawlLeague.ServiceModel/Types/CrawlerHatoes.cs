using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{

    public class CrawlerHatoes : Crawler
    {
        [Description("References to peripheral resources")] private readonly CrawlerRefs _references = new CrawlerRefs();
        public CrawlerRefs References { get { return _references; } }
    }

    public class CrawlerRefs
    {
        [Description("Link to the server that the crawler is currently bound to")]
        public string ServerRef { get; set; }

        [Description("Link to the division that the crawler is slotted into as of the last divison realignment")]
        public string DivisionRef { get; set; }   
    }
}
