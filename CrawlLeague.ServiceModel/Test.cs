using ServiceStack;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    [Route("/test")]
    public class Test
    {
        [AutoIncrement] 
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
