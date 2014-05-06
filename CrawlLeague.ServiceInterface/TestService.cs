using CrawlLeague.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;

namespace CrawlLeague.ServiceInterface
{
    public class TestService : Service
    {
        public object Get(Test request)
        {
            return new Test {Id = 1};
        }

        public object Post(Test testObj)
        {
            Db.Save(testObj);
            return true;
        }
    }
}
