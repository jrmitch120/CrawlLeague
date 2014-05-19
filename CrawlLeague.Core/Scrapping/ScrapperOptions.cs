namespace CrawlLeague.Core.Scrapping
{
    public class ScrapperOptions
    {
        public ByteRange Range { get; set; }
    }

    public class ByteRange
    {
        public ByteRange(int start, int end)
        {
            Start = start;
            End = end;
        }
        public int Start { get; private set; }
        public int End { get; private set; }
    }
}
