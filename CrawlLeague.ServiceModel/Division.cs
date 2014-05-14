using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    public class Division
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
