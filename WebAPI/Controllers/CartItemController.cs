using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : Controller
    {
        private readonly ICartService cartService;

        public CartItemController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet("all")]
        public IActionResult GetAll() => Ok(this.cartService.GetAll());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id) => Ok(await this.cartService.Get(id));

        [HttpGet("{userId}/cart")]
        public IActionResult GetByUserId([FromRoute] string userId) => Ok(this.cartService.GetByUser(userId));

        [HttpPost]
        public IActionResult Create([FromForm] CreateCartItemModel createCartItemModel)
        {
            cartService.Create(createCartItemModel);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await cartService.Delete(id);
            return Ok();
        }

        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            await cartService.ClearCart(userId);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] EditCartItemModel editCartItemModel)
        {
            await cartService.Edit(editCartItemModel);
            return Ok();
        }
    }
}
