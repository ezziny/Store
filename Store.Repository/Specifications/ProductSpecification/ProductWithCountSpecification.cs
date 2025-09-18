using Store.Data.Entities;

namespace Store.Repository.Specifications.ProductSpecification;

public class ProductWithCountSpecification: BaseSpecification<Product>
{
    public ProductWithCountSpecification(ProductSpecification specification) : base(product => (!specification.BrandId.HasValue || product.BrandId == specification.BrandId.Value) &&
                                                                                               (!specification.TypeId.HasValue || product.TypeId == specification.TypeId.Value) &&
                                                                                               (string.IsNullOrEmpty(specification.Search) || product.Name.Trim().ToLower().Contains(specification.Search))

    )
    {
        
    }

}
 