using System;

namespace CrawlLeague.ServiceModel
{
    public interface IAudit :ICreated
    {
        DateTime ModifiedDate { get; set; }
    }
}