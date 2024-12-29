using business_logic.DTOs;
using business_logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        [HttpGet("all")]
        public IActionResult GetAll() => Ok(this._productService.GetAll());

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            return  Ok(await _productService.Get(id));
        }

        [HttpPost]
        public IActionResult Create([FromForm] CreateProductModel createProductModel)
        {
            _productService.Create(createProductModel);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] ProductDto product)
        {
            await _productService.Edit(product);
            return Ok();
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _productService.Delete(id);
            return Ok();
        }
    }
}
