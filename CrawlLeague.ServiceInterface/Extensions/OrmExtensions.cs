using CrawlLeague.ServiceModel.Util;
using ServiceStack.OrmLite;

namespace CrawlLeague.ServiceInterface.Extensions
{
    public static class OrmExtensions
    {
        public static SqlExpression<T> PageTo<T>(this SqlExpression<T> expression, int page)
        {
            expression.Offset = (page - 1)*Paging.PageSize;
            expression.Rows = Paging.PageSize;
            
            return expression;
        }

        public static bool Search(this string target, string search)
        {
            if (search.StartsWith("*") && search.EndsWith("*"))
                return (target.Contains(search.Trim(new[] {'*'})));
            if(search.EndsWith("*"))
                return (target.StartsWith(search.TrimEnd(new[] { '*' })));
            if (search.StartsWith("*"))
                return (target.EndsWith(search.TrimStart(new[] { '*' })));

            return (target.Equals(search));
        }
    }
}
