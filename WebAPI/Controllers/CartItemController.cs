using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : Controller
    {
        private readonly ICartService _cartService;

        public CartItemController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetAll()
        {
            var cartItems = await _cartService.GetAll();
            return Ok(cartItems);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id) => Ok(await _cartService.Get(id));

        [HttpGet("{userId}/cart")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetByUserId([FromRoute] string userId)
        {
            var cartItems = await _cartService.GetByUser(userId);
            return Ok(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCartItemModel createCartItemModel)
        {
            await _cartService.Create(createCartItemModel);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _cartService.Delete(id);
            return Ok();
        }

        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            await _cartService.ClearCart(userId);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] EditCartItemModel editCartItemModel)
        {
            await _cartService.Edit(editCartItemModel);
            return Ok();
        }
    }
}