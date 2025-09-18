using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;

namespace Store.Repository.Specifications;

public class SpecificationEvaluater<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
    {
        var query = inputQuery;
        if (specification.Criteria is not null)
            query = query.Where(specification.Criteria);
        if (specification.OrderBy is not null)
            query = query.OrderBy(specification.OrderBy);
        if (specification.OrderByDescending is not null)
            query = query.OrderByDescending(specification.OrderByDescending);
        query = specification.Includes.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
        if (specification.IsPaginated)
            query = query.Skip(specification.Skip).Take(specification.Take);
        return query;
    }
}