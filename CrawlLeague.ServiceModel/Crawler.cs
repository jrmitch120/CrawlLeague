using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel
{
    public class Crawler : ICreated
    {
        [AutoIncrement]
        public int Id { get; set; }
        
        public string UserName { get; set; }

        // TODO, division...

        public DateTime CreatedDate { get; set; }
    }
}
