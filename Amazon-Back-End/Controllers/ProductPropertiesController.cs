using business_logic.DTOs;
using business_logic.DTOs;
using business_logic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Amazon_Back_End.Helpers.SeedExtension;

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public IActionResult Create([FromBody] CreateProductPropertiesModel createProductPropertiesModel)
        {
            _productPropertiesService.Create(createProductPropertiesModel);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromBody] IEnumerable<int> propertyIds)
        {
            await _productPropertiesService.Delete(id, propertyIds);
            return Ok();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Edit([FromBody] EditProductPropertiesModel editProductPropertiesModel)
        {
            await _productPropertiesService.Edit(editProductPropertiesModel);
            return Ok();
        }

    }
}
