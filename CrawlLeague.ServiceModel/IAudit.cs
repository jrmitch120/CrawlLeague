using System;

namespace CrawlLeague.ServiceModel
{
    public interface IAudit 
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }
    }
}