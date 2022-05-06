using BiPapyon.Api.Application.Interfaces.Repositories;
using BiPapyon.Api.Domain.Models;
using BiPapyon.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BiPapyon.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbContext dbContext;

        protected DbSet<T> entity => dbContext.Set<T>();

        public GenericRepository(DbContext context)
        {
            this.dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Insert

        public virtual int Add(T entity)
        {
            this.entity.Add(entity);

            return this.dbContext.SaveChanges();
        }

        public virtual int Add(IEnumerable<T> entities)
        {
            entity.AddRange(entities);

            return dbContext.SaveChanges();
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            await this.entity.AddAsync(entity);

            return await dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> AddAsync(IEnumerable<T> entities)
        {
            await entity.AddRangeAsync(entities);

            return await dbContext.SaveChangesAsync();
        }

        #endregion

        #region Update

        public virtual int Update(T entity)
        {
            this.entity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;

            return dbContext.SaveChanges();
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            this.entity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;

            return await dbContext.SaveChangesAsync();
        }
        #endregion

        #region Delete

        public virtual int Delete(T entity)
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);

            return dbContext.SaveChanges();
        }

        public int Delete(Guid id)
        {
            var entity = this.entity.Find(id);

            return Delete(entity);
        }
        public virtual async Task<int> DeleteAsync(Guid id)
        {
            var entity = this.entity.Find(id);

            return await DeleteAsync(entity);
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);

            return await dbContext.SaveChangesAsync();
        }

        public virtual int Delete(IEnumerable<T> entities)
        {
            dbContext.RemoveRange(entities);

            return dbContext.SaveChanges();
        }
        public virtual async Task<int> DeleteAsync(IEnumerable<T> entities)
        {
            dbContext.RemoveRange(entities);

            return await dbContext.SaveChangesAsync();
        }

        public virtual bool DeleteRange(Expression<Func<T, bool>> predicate)
        {
            dbContext.RemoveRange(entity.Where(predicate));

            return dbContext.SaveChanges() > 0;
        }

        public virtual async Task<bool> DeleteRangeAsync(Expression<Func<T, bool>> predicate)
        {
            dbContext.RemoveRange(entity.Where(predicate));

            return await dbContext.SaveChangesAsync() > 0;
        }

        #endregion

        #region Add or Update

        public virtual int AddOrUpdate(T entity)
        {
            if (!this.entity.Local.Any(x => EqualityComparer<Guid>.Default.Equals(x.Id, entity.Id)))
                dbContext.Update(entity);

            return dbContext.SaveChanges();
        }

        public virtual async Task<int> AddOrUpdateAsync(T entity)
        {
            if (!this.entity.Local.Any(x => EqualityComparer<Guid>.Default.Equals(x.Id, entity.Id)))
                dbContext.Update(entity);

            return await dbContext.SaveChangesAsync();
        }


        #endregion

        #region Get
        public IQueryable<T> AsQueryable() => entity.AsQueryable();

        public virtual Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            return Get(predicate, noTracking, includes).FirstOrDefaultAsync();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return query;
        }

        public Task<List<T>> GetAll(bool noTracking = false)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            T found = await entity.FindAsync(id);

            if (found == null)
                return null;

            if (noTracking)
                dbContext.Entry(found).State = EntityState.Detached;

            foreach (var include in includes)
            {
                dbContext.Entry(found).Reference(include).Load();
            }

            return found;
        }

        public virtual async Task<List<T>> GetList(Expression<Func<T, bool>> predicate, bool noTracking = true, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = entity;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (Expression<Func<T, object>> include in includes)
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = entity;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync();
        }

        #endregion

        #region Bulk


        public virtual Task BulkAdd(IEnumerable<T> entities)
        {
            if (entities != null && !entities.Any())
                return Task.CompletedTask;

            foreach (var item in entities)
            {
                entity.Add(item);
            }

            return dbContext.SaveChangesAsync();
        }

        public virtual Task BulkDelete(Expression<Func<T, bool>> entities)
        {
            IQueryable<T> query = entity;

            if (entities != null)
            {
                query = entity.Where(entities);
            }

            dbContext.RemoveRange(query);

            return dbContext.SaveChangesAsync();
        }

        public virtual Task BulkDelete(IEnumerable<T> entities)
        {
            dbContext.RemoveRange(entities);

            return dbContext.SaveChangesAsync();
        }

        public virtual Task BulkDeleteById(IEnumerable<Guid> ids)
        {
            if (ids != null && !ids.Any())
                return Task.CompletedTask;

            dbContext.RemoveRange(entity.Where(x => ids.Contains(x.Id)));

            return dbContext.SaveChangesAsync();
        }

        public Task BulkUpdate(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        #endregion
        
        private static IQueryable<T> ApplyIncludes(IQueryable<T> query, Expression<Func<T, object>>[] includes)
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;

        }
    }
}