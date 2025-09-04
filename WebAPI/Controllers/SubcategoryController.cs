using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static WebAPI.Helpers.SeedExtension;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : Controller
    {
        private readonly ISubcategoryService _subcategoryService;

        public SubcategoryController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<SubcategoryDto>>> GetAll()
        {
            var subcategories = await _subcategoryService.GetAll();
            return Ok(subcategories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await _subcategoryService.Get(id));
        }

        [HttpGet("{id:int}/subcategories")]
        public async Task<ActionResult<IEnumerable<SubcategoryDto>>> GetSubcategoriesByCategory([FromRoute] int id)
        {
            var subcategories = await _subcategoryService.GetSubcategoriesByCategoryAsync(id);
            return Ok(subcategories);
        }

        [HttpGet("category-{categoryName}/subcategories")]
        public async Task<ActionResult<IEnumerable<SubcategoryDto>>> GetSubcategoriesByCategory([FromRoute] string categoryName)
        {
            var subcategories = await _subcategoryService.GetSubcategoriesByCategoryNameAsync(categoryName);
            return Ok(subcategories);
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Create([FromBody] CreateSubcategoryModel model)
        {
            await _subcategoryService.Create(model); // Правильно await'имо асинхронний метод
            return Ok();
        }

        [HttpPut]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Edit([FromBody] EditSubcategoryModel model)
        {
            await _subcategoryService.Edit(model);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _subcategoryService.Delete(id);
            return Ok();
        }
    }
}