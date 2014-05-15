using System;
using System.Net;

namespace CrawlLeague.Core.Scrapping
{
    public class UriRequestRunner : IScraperRequestRunner
    {
        public string Fetch(Uri uri)
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadString(uri);
            }
        }
    }
}