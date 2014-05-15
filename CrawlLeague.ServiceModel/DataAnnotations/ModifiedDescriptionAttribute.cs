using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.DataAnnotations
{
    public class ModifiedAttribute : DescriptionAttribute
    {
        public ModifiedAttribute() : base("The last time the resource was modified (UTC)")
        {
        }
    }
}
