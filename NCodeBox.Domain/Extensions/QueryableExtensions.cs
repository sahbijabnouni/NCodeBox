using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NCodeBox.Domain.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class QueryableExtensions
    {
        public static PaginatedList<T> ToPaginatedList<T>(
            this IQueryable<T> query, int pageIndex, int pageSize)
        {

            var totalCount = query.Count();
            var collection = query;

            if (totalCount > (pageIndex * pageSize))
            {
                collection = query.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                pageIndex = 0;
            }

            return new PaginatedList<T>(collection.ToList(), pageIndex, pageSize, totalCount);
        }
        public static PaginatedList<T> ToPaginatedList<T>(
            this IList<T> collection, int pageIndex, int pageSize,int pageCount)
        {
            return new PaginatedList<T>(collection, pageIndex, pageSize, pageCount);
        }
    }
}
