using BLL.DTOs;
using BLL.DTOs.Wishlist;
using BLL.Entities;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishListService;

        public WishlistController(IWishlistService wishListService)
        {
            _wishListService = wishListService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishList(string userId)
        {
            var items = await _wishListService.GetWishlistById(userId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishList([FromForm] CreateWishlistModel itemDto)
        {
            if (itemDto == null)
            {
                return BadRequest("Invalid item.");
            }

            await _wishListService.Create(itemDto);
            return Ok("Item added to wishlist.");
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] EditWishlistModel model)
        {
            await _wishListService.Edit(model);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveWishlist(int id)
        {
            await _wishListService.Delete(id);
            return Ok("Item removed from wishlist.");
        }
    }
}
