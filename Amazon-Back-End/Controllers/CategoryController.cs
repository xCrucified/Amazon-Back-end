using business_logic.DTOs;
using business_logic.Interfaces;
using business_logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        public readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("all")]
        public IActionResult GetAll() => Ok(this.categoryService.GetAll());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute]int id) =>  Ok(await this.categoryService.Get(id));

        [HttpPost]
        public IActionResult Create([FromForm] CreateCategoryModel createCategoryModel)
        {
            categoryService.Create(createCategoryModel);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditCategoryModel category)
        {
            await categoryService.Edit(category);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await categoryService.Delete(id);
            return Ok();
        }
    }
}
