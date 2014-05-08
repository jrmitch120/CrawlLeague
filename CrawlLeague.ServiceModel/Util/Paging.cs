namespace CrawlLeague.ServiceModel.Util
{
    public class Paging
    {
        public static int PageSize = 25;

        public int Page { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get { return (TotalCount + PageSize - 1) / PageSize; } }
    }
}
