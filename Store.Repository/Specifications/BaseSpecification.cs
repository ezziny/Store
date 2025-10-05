using System.Linq.Expressions;

namespace Store.Repository.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }
    public Expression<Func<T, bool>> Criteria { get; }

    public List<Expression<Func<T, object>>> Includes { get; } = [];


    protected void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);
    public Expression<Func<T, object>> OrderBy { get; private set; }

    public Expression<Func<T, object>> OrderByDescending { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPaginated { get; private set; }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression) => OrderByDescending = orderByDescendingExpression;
    protected void ApplyPagination(int skip, int take)
    {
        Take = take;
        Skip = skip;
        IsPaginated = true;
    }
}
