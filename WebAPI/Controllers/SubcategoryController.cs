using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
using BLL.Specifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static WebAPI.Helpers.SeedExtension;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPI.Controllers
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
        public IActionResult GetAll() => Ok(this.subcategoryService.GetAll());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await subcategoryService.Get(id));
        }

        [HttpGet("{id:int}/subcategories")]
        public async Task<ActionResult> GetSubcategoriesByCategory([FromRoute] int id)
        {
            var subcategories = await subcategoryService.GetSubcategoriesByCategoryAsync(id);

            return Ok(subcategories);
        }

        [HttpGet("category-{categoryName}/subcategories")]
        public async Task<ActionResult> GetSubcategoriesByCategory([FromRoute] string categoryName)
        {
            var subcategories = await subcategoryService.GetSubcategoriesByCategoryNameAsync(categoryName);

            return Ok(subcategories);
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
