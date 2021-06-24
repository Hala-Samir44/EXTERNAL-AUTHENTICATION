using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EX_AUTH_BE.DAL.Repositories
{
    public class Repository<TEntity> where TEntity : class
    {
        protected readonly ExternalAuthContext Context;

        public Repository(ExternalAuthContext context)
        {
            this.Context = context;
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }
        public List<TEntity> GetAllById(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }
        public bool RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
            var v = Context.SaveChanges();
            return Context.SaveChanges() > 0;
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        public ValueTask<TEntity> GetByIdAsync(int id)
        {
            return Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);

            return Context.SaveChanges() > 0 ? entity : null;
        }

        public bool Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);

            return Context.SaveChanges() > 0;
        }

        public TEntity Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);

            return Context.SaveChanges() > 0 ? entity : null;
        }

    }
}
