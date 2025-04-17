using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation
{
    [ApiController]
    [Route(template: "api /[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        // endPoint : public non static method

        [HttpGet] //endPoint : GET : /api/products
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductSpecificationsParameters SpecParam)
        {
            var result = await serviceManager.ProductService.GetAllProductsAsync(SpecParam);

            if (result is null) return BadRequest();//400
            return Ok(result);  //200

        }


        [HttpGet ("{id}")] // GET : /api /products/12
        public async Task<IActionResult> GetProductById(int id)
        {
             var result = await serviceManager.ProductService.GetProductByIdAsync(id);
            if (result is null) return NotFound(); //404
            return Ok(result); //200
        }

        [HttpGet("brands")]  //TODO :Get All Brands
        public async Task<IActionResult> GetAllBrands()
        {
             var result = await serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest();    
            return Ok(result);  
        }

        [HttpGet("types")]   //TODO :Get All Types
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest(); //400
            return Ok(result);//200
        }
    }
}
