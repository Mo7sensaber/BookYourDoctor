using Domain.Model;
using Domain.RepoInterface;
using Microsoft.EntityFrameworkCore;
using Persistanse.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistanse.Repository
{
    public class GenaricRepository<TEntity, TKey> : IGenaricRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly ContextBooking context;

        public GenaricRepository(ContextBooking context) 
        {
            this.context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }

        public async Task Delete(TKey id)
        {
            var entity =await context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                context.Set<TEntity>().Remove(entity);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }


        public async Task Edit(TEntity entity)
        {
            var Entity=await context.Set<TEntity>().FindAsync(entity.Id);
            if (Entity != null)
            {
                context.Set<TEntity>().Update(entity);
            }

        }

        public async Task<TEntity?> GetByIdSpecAsync(ISpecification<TEntity, TKey> specification)
        {
            return await SpecificationEvaluator.GetQuery(context.Set<TEntity>(), specification).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllSpecAsync(ISpecification<TEntity, TKey> specification)
        {
            return await SpecificationEvaluator.GetQuery(context.Set<TEntity>(), specification).ToListAsync();
        }
    }
}
