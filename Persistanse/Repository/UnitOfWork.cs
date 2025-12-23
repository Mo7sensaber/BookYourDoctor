using Domain.Model;
using Domain.RepoInterface;
using Persistanse.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistanse.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContextBooking context;
        private readonly Dictionary<string, object> repositories = new Dictionary<string, object>();
        public UnitOfWork(ContextBooking context) 
        {
            this.context = context;
        }
        public IGenaricRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var type = typeof(TEntity).Name;
            if (repositories.ContainsKey(type))
            {
                return (IGenaricRepository<TEntity, Tkey>)repositories[type];
            }
            var repository = new GenaricRepository<TEntity, Tkey>(context);
            repositories.Add(type, repository);
            return repository;
        }

        public async Task<int> SaveChanges()
        {
            return await context.SaveChangesAsync();
        }
    }
}
