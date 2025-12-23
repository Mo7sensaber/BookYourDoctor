using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepoInterface
{
    public interface IGenaricRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdSpecAsync(ISpecification<TEntity, TKey> specification);
        Task<IEnumerable<TEntity>> GetAllSpecAsync(ISpecification<TEntity, TKey> specification);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task AddAsync(TEntity entity);
        Task Edit(TEntity entity);
        Task Delete(TKey id);
    }
}
