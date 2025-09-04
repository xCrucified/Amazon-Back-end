using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
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
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll()
        {
            var reviews = await _reviewService.GetAll();
            return Ok(reviews);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReviewDto>> Get([FromRoute] int id)
        {
            return Ok(await _reviewService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateReviewModel model)
        {
            await _reviewService.Create(model);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Delete(int id)
        {
            await _reviewService.Delete(id);
            return Ok();
        }
    }
}