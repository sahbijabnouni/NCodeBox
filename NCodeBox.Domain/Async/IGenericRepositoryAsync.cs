using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NCodeBox.Domain.Async
{
    public interface IGenericRepositoryAsync<E, T>
        where E : class, IIdentity<T>
    {
        Task<IList<E>> GetAsync(
            Expression<Func<E, bool>> filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>> orderBy = null,
            params Expression<Func<E, object>>[] includes);
        PaginatedListAsync<E> GetPaginatedAsync(int? pageIndex, int? pageSize, Expression<Func<E, bool>> filter = null, Func<IQueryable<E>, IOrderedQueryable<E>> orderBy = null, params Expression<Func<E, object>>[] includes);
        Task<E> GetSingleAsync(Expression<Func<E, bool>> filter = null, params Expression<Func<E, object>>[] includes);
        Task<E> GetByIdAsync(T id, params Expression<Func<E, object>>[] includes);
        void Insert(E entity);
        void Delete(T id);
        void Delete(E entityToDelete);
        void Update(E entityToUpdate);
    }
}
