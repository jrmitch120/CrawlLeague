using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.DataAnnotations
{
    public class CreatedAttribute : DescriptionAttribute
    {
        public CreatedAttribute()
            : base("When the resource was added (UTC)")
        {
        }
    }
}
