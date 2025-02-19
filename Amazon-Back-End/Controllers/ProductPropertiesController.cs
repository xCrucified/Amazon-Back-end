using business_logic.DTOs;
using business_logic.DTOs;
using business_logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPropertiesController : Controller
    {
        public readonly IProductPropertiesService _productPropertiesService;

        public ProductPropertiesController(IProductPropertiesService productProperties)
        {
            this._productPropertiesService = productProperties;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateProductPropertiesModel createProductPropertiesModel)
        {
            _productPropertiesService.Create(createProductPropertiesModel);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromBody] IEnumerable<int> propertyIds)
        {
            await _productPropertiesService.Delete(id, propertyIds);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditProductPropertiesModel editProductPropertiesModel)
        {
            await _productPropertiesService.Edit(editProductPropertiesModel);
            return Ok();
        }

    }
}
