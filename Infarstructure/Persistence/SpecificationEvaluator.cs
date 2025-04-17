using System;
using Domain.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public  static class SpecificationEvaluator
    {
        //Generate Query
        public static IQueryable<TEntity> GetQuery<TEntity, Tkey>
             (IQueryable<TEntity> inputQuery,
             ISpecifications<TEntity, Tkey> spec)
             where TEntity : BaseEntity<Tkey>
        {

            var Query = inputQuery;

            if (spec.Criteria is not null)
            {
                Query = Query.Where(spec.Criteria);
            }

            if (spec.OrderBy is not null)
                Query = Query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDesc is not null)
                Query = Query.OrderByDescending(spec.OrderByDesc);


            if (spec.IsPagination)
            {
                Query = Query.Skip(spec.Skip).Take(spec.take);
            }


            Query = spec.IncludeExpressions.Aggregate(Query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return Query;
        }
    }
}
//  _context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync() as  IEnumerable<TEntity>