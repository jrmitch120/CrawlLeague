using System;

namespace CrawlLeague.Core.Scrapping
{
    public interface IScraperRequestRunner
    {
        string Fetch(Uri uri, ScrapperOptions options);
    }
}