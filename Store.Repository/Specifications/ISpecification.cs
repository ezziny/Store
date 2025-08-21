using System;
using System.Linq.Expressions;

namespace Store.Repository.Specifications;

public interface ISpecification<T>
{
    #region Criteria
    Expression<Func<T, bool>> Criteria { get; }
    #endregion
    #region Includes
    List<Expression<Func<T, object>>> Includes { get; }
    #endregion
    #region OrderBy
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDescending { get; }
    #endregion
    #region Pagination
    int Take { get; }
    int Skip { get; }
    bool IsPaginated { get; }
    #endregion

}
