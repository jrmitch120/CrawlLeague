using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    public class Server
    {
        [AutoIncrement]
        public virtual int Id { get; set; }

        public string Name { get; set; }

        [Index(Unique=true)]
        public string Abbreviation { get; set; }

        public string Address { get; set; }
    }
}
