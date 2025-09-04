using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static WebAPI.Helpers.SeedExtension;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id) => Ok(await _categoryService.Get(id));

        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Create([FromForm] CreateCategoryModel createCategoryModel)
        {
            await _categoryService.Create(createCategoryModel);
            return Ok();
        }

        [HttpPut]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Edit([FromBody] EditCategoryModel category)
        {
            await _categoryService.Edit(category);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _categoryService.Delete(id);
            return Ok();
        }
    }
}