using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using NCodeBox.Domain.Async;

namespace NCodeBox.Domain.EF.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class QueryableExtensions
    {
        public static PaginatedListAsync<T> ToPaginatedListAsync<T>(
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

            return new PaginatedListAsync<T>(collection.ToListAsync(), pageIndex, pageSize, totalCount);
        }
        
    }
}
