using Store.Data.Entities;

namespace Store.Repository.Specifications.ProductSpecification;

public class ProductWithSpecification : BaseSpecification<Product>
{
    public ProductWithSpecification(ProductSpecification specification) : base(product => (!specification.BrandId.HasValue || product.BrandId == specification.BrandId.Value) &&
                                                                                         (!specification.TypeId.HasValue || product.TypeId == specification.TypeId.Value) &&
                                                                                         (string.IsNullOrEmpty(specification.Search) || product.Name.Trim().ToLower().Contains(specification.Search))
    )
    {
        AddInclude(x => x.Brand);
        AddInclude(x => x.Type);
        AddOrderBy(x => x.Name);
        if (!string.IsNullOrEmpty(specification.Sort))
        {
            Action action = specification.Sort switch
            {
                "priceAsc" => () => AddOrderBy(x => x.Price),
                "priceDesc" => () => AddOrderByDescending(x => x.Price),
                _ => () => AddOrderBy(x => x.Name)
            };

            action();
        }
        ApplyPagination(specification.PageSize * (specification.PageIndex - 1), specification.PageSize);
    }
    public ProductWithSpecification(int? id) : base(product => product.Id == id)
    {
        AddInclude(x => x.Brand);
        AddInclude(x => x.Type);
    }
    }
