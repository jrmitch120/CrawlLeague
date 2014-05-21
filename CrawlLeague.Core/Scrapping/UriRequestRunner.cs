using System;
using System.IO;
using System.Net;

namespace CrawlLeague.Core.Scrapping
{
    public class UriRequestRunner : IScraperRequestRunner
    {
        public string Fetch(Uri uri, ScrapperOptions options)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            
            if(options != null && options.Range != null)
                request.AddRange(options.Range.Start, options.Range.End);
           
            WebResponse response = request.GetResponse();

            TextReader body = new StreamReader(response.GetResponseStream());
            
            return (body.ReadToEnd());
        }
    }
}