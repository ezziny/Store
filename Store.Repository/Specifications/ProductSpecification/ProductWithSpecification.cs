using System;
using Store.Data.Entities;

namespace Store.Repository.Specifications.ProductSpecification;

public class ProductWithSpecification : BaseSpecification<Product>
{
    public ProductWithSpecification(ProductSpecification specification) : base(product => (!specification.BrandId.HasValue || product.BrandId == specification.BrandId.Value) &&
                                                                                         (!specification.TypeId.HasValue || product.TypeId == specification.TypeId.Value)
    )
    {
        addInclude(x => x.Brand);
        addInclude(x => x.Type);
        AddOrderBy(x => x.Name);
        if (!string.IsNullOrEmpty(specification.Sort))
        {
            switch (specification.Sort)
            {
                case "priceAsc":
                    AddOrderBy(x => x.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(x => x.Price);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;


            }
        }
        ApplyPagination(specification.PageSize * (specification.PageIndex - 1), specification.PageSize);
    }
    public ProductWithSpecification(int? id) : base(product => product.Id == id)
    {
        addInclude(x => x.Brand);
        addInclude(x => x.Type);
    }
    }
