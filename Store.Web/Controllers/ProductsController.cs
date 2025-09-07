using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Repository.Specifications.ProductSpecification;
using Store.Services.Services.Product;
using Store.Services.Services.Product.Dtos;
using Store.Web.Helper;

namespace Store.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllBrands() => Ok(await _productService.GetAllBrandsAsync());
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllTypes() => Ok(await _productService.GetAllTypesAsync());
        [HttpGet]
        [Cache(15)]
        public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetAllProducts([FromQuery] ProductSpecification input) => Ok(await _productService.GetAllProductsAsync(input));
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetProductById(int? id) => Ok(await _productService.GetProductByIdAsync(id));

    }
}
