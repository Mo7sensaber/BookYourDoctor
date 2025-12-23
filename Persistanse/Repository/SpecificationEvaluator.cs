using Domain.Model;
using Domain.RepoInterface;
using Microsoft.EntityFrameworkCore;
using Service.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistanse.Repository
{
    static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity,Tkey>(IQueryable<TEntity> inputQuery, ISpecification<TEntity,Tkey> spec)where TEntity : BaseEntity<Tkey>
        {
            var query = inputQuery;
            // Apply criteria
            if (spec.Creteria != null)
            {
                query = query.Where(spec.Creteria);
            }
            // Apply includes

            if (spec.Includes is not null && spec.Includes.Count > 0)
            {
                foreach (var Exp in query)
                {
                    query = spec.Includes.Aggregate(query, (CurrentQuery, Expression) => CurrentQuery.Include(Expression));
                }
            }            // Apply ordering

            return query;
        }
    }
}
