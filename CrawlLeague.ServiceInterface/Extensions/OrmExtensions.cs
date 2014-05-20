using System;
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
    }
}
