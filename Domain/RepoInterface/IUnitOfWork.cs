using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepoInterface
{
    public interface IUnitOfWork
    {
        public IGenaricRepository<TEntity,Tkey> Repository<TEntity,Tkey>() where TEntity : BaseEntity<Tkey>;
        Task<int> SaveChanges();
    }
}
