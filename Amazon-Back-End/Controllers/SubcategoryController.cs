using business_logic.DTOs;
using business_logic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Amazon_Back_End.Helpers.SeedExtension;

namespace Amazon_Back_End.Controllers
{
    [Route("api/[controller]")]
    public class SubcategoryController : Controller
    {
        private readonly ISubcategoryService subcategoryService;

        public SubcategoryController(ISubcategoryService subcategoryService)
        {
            this.subcategoryService = subcategoryService;
        }

        [HttpGet("all")]
        public IActionResult GetAll() => Ok(subcategoryService.GetAll());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await subcategoryService.GetById(id));
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public IActionResult Create([FromBody] CreateSubcategoryModel model)
        {
            subcategoryService.Create(model);
            return Ok();
        }

        [HttpPut]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Edit([FromBody] EditSubcategoryModel model)
        {
            await subcategoryService.Edit(model);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await subcategoryService.Delete(id);
            return Ok();
        }
    }
}
