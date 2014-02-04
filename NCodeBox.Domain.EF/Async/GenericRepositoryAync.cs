using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NCodeBox.Domain.Async;
using NCodeBox.Domain.EF.Extensions;

namespace NCodeBox.Domain.EF.Async
{
    public class GenericRepositoryAsync<TEntity, TId> : IGenericRepositoryAsync<TEntity, TId> where TEntity : class, IIdentity<TId>
    {
        internal DbContext Context;
        internal DbSet<TEntity> DbSet;

        public GenericRepositoryAsync(DbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return await GetQueryable(filter , orderBy ,includes).ToListAsync();
        }

        public virtual PaginatedListAsync<TEntity> GetPaginatedAsync(int? pageIndex, int? pageSize, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            pageIndex = pageIndex ?? 0;
            pageSize = pageSize ?? 10;

            var queryable = GetQueryable(filter, orderBy, includes);
            if (orderBy == null) queryable = queryable.OrderByDescending(x => x.Id);
            var paginatedList = queryable.ToPaginatedListAsync((int)pageIndex, (int)pageSize);

            return paginatedList;
        }

        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = AddIncludes(query, includes);
            }
            return await query.SingleOrDefaultAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(TId id, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = AddIncludes(DbSet, includes);
            return await query.SingleOrDefaultAsync(i => (object)i.Id == (object)id);
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(TId id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        private IQueryable<TEntity> AddIncludes(IQueryable<TEntity> query, IEnumerable<Expression<Func<TEntity, object>>> includes)
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            }
            return query;
        }
        private  IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = AddIncludes(query, includes);
            }

            return orderBy != null ? orderBy(query) : query;
        }
    }
}