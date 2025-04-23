using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Abstractions;
using ServicesAbstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet] // api/basket?id=1
        public async Task<IActionResult> GetBasketById(string id)
        {
            var result = await serviceManager.basketService.GetBasketAsync(id);
            return Ok(result);
        }


        [HttpPost] //api/basket
        public async Task<IActionResult> UpdateBasket(BasketDto basketDto)
        {
            var result = await serviceManager.basketService.UpdateBasketAsync(basketDto);
            return Ok(result);
        }

        [HttpDelete] // api/basket?id
        public async Task<IActionResult> DeleteBasket(string id)
        {
            await serviceManager.basketService.DeleteBasketAsync(id);
            return NoContent();
        }

    }
}