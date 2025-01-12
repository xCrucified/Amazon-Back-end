using business_logic.DTOs;
using business_logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        public readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("all")]
        public IActionResult GetAll() => Ok(this.productService.GetAll());

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            return  Ok(await productService.Get(id));
        }

        [HttpPost]
        public IActionResult Create([FromForm] CreateProductModel createProductModel)
        {
            productService.Create(createProductModel);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] EditProductModel product)
        {
            await productService.Edit(product);
            return Ok();
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await productService.Delete(id);
            return Ok();
        }
    }
}
