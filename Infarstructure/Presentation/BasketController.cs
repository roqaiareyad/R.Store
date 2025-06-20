using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using ServicesAbstractions;
using Shared;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/baskets")]
    public class BasketController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public BasketController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        [HttpGet] // GET api/baskets?id=123
        public async Task<IActionResult> GetBasketById([FromQuery] string id)
        {
            var result = await serviceManager.BasketService.GetBasketAsync(id);
            return Ok(result);
        }

        [HttpPost] // POST api/baskets
        public async Task<IActionResult> UpdateBasket([FromBody] BasketDto basketDto)
        {
            var result = await serviceManager.BasketService.UpdateBasketAsync(basketDto);
            return Ok(result);
        }

        [HttpDelete] // DELETE api/baskets?id=123
        public async Task<IActionResult> DeleteBasket([FromQuery] string id)
        {
            await serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
