using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.BasketService;
using Store.Services.Services.BasketService.Dtos;

namespace Store.Web.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketAsync(string id) => Ok(await _basketService.GetBasketAsync(id));

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasketAsync(CustomerBasketDto id) => Ok(await _basketService.UpdateBasketAsync(id));
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasketAsync(string id) => await _basketService.DeleteBasketAsync(id);
    }
}
