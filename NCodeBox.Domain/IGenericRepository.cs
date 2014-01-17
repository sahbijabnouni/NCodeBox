using System;
using System.Linq;
using System.Linq.Expressions;

namespace NCodeBox.Domain
{
    public interface IGenericRepository<E, T>
        where E : class, IIdentity<T>
    {
        IQueryable<E> Get(
            Expression<Func<E, bool>> filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>> orderBy = null,
            params Expression<Func<E, object>>[] includes);
        PaginatedList<E> GetPaginated(int? pageIndex, int? pageSize, Expression<Func<E, bool>> filter = null, Func<IQueryable<E>, IOrderedQueryable<E>> orderBy = null, params Expression<Func<E, object>>[] includes);
        E GetSingle(Expression<Func<E, bool>> filter = null, params Expression<Func<E, object>>[] includes);
        E GetById(T id, params Expression<Func<E, object>>[] includes);
        void Insert(E entity);
        void Delete(T id);
        void Delete(E entityToDelete);
        void Update(E entityToUpdate);
    }
}