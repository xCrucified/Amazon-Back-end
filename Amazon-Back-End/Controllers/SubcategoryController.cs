using business_logic.DTOs;
using business_logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Create([FromBody] CreateSubcategoryModel model)
        {
            subcategoryService.Create(model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditSubcategoryModel model)
        {
            await subcategoryService.Edit(model);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await subcategoryService.Delete(id);
            return Ok();
        }
    }
}
