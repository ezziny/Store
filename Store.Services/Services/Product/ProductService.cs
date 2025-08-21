using System;
using AutoMapper;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specifications;
using Store.Repository.Specifications.ProductSpecification;
using Store.Services.Helper;
using Store.Services.Services.Product.Dtos;

namespace Store.Services.Services.Product;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync()
    {
        var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync();
        var mappedBrands = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(brands); //Map<Destination>(source)  
        return mappedBrands;
    }

    public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification input)
    {
        var specs = new ProductWithSpecification(input); 
        var products = await _unitOfWork.Repository<Store.Data.Entities.Product, int>().GetAllWithSpecificationAsync(specs);
        var countWithSpecs = new ProductWithCountSpecification(input);
        var count = await _unitOfWork.Repository<Data.Entities.Product, int>().GetCountWithSpecificationAsync(countWithSpecs);
        var mappedProducts = _mapper.Map<IReadOnlyList<ProductDetailsDto>>(products);
        //get the total count of rows in the WHOLE TABLE
        // int count = await _unitOfWork.Repository<Data.Entities.Product, int>().GetCountAsync();
        return new PaginatedResultDto<ProductDetailsDto>(input.PageIndex, input.PageSize, count, mappedProducts);
    }

    public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync()
    {
        var types = await _unitOfWork.Repository<ProductType, int>().GetAllAsync();
        var mappedTypes = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(types);
        return mappedTypes;
    }

    public async Task<ProductDetailsDto> GetProductByIdAsync(int? id)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id), "Product ID cannot be null.");
        var specs = new ProductWithSpecification(id);

        var product = await _unitOfWork.Repository<Data.Entities.Product, int>().GetWithSpecificationByIdAsync(specs);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {id} not found.");

        var mappedProduct = _mapper.Map<ProductDetailsDto>(product);
        return mappedProduct;
    }


}
