using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static WebAPI.Helpers.SeedExtension;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewService reviewService_;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService_ = reviewService;
        }


        [HttpGet("all")]
        public IActionResult GetAll() => Ok(reviewService_.GetAll());

        [HttpGet("{id::int}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            return Ok(await reviewService_.Get(id));
        }

        [HttpPost]
        public IActionResult Create([FromForm] CreateReviewModel model)
        {
            reviewService_.Create(model);
            return Ok();
        }
        
        [HttpDelete("{id::int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Delete(int id)
        {
            await reviewService_.Delete(id);
            return Ok();
        }
    }
}
