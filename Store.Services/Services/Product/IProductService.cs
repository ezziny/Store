using System;
using Store.Repository.Specifications.ProductSpecification;
using Store.Services.Helper;
using Store.Services.Services.Product.Dtos;

namespace Store.Services.Services.Product;

public interface IProductService
{
    Task<ProductDetailsDto> GetProductByIdAsync(int? id);
    Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification specification);
    Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync();
    Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync();

}
