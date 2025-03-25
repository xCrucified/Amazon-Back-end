using business_logic.DTOs;
using business_logic.Interfaces;
using business_logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_Back_End.Controllers
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
        public async Task<IActionResult> GetWishList(int userId)
        {
            var items = await _wishListService.GetWishlistItemById(userId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishList([FromBody] WishListItemDto itemDto)
        {
            if (itemDto == null)
            {
                return BadRequest("Invalid item.");
            }

            await _wishListService.AddWishlistItem(itemDto);
            return Ok("Item added to wishlist.");
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromWishList(string userId, int productId)
        {
            await _wishListService.RemoveWishlistItem(userId, productId);
            return Ok("Item removed from wishlist.");
        }
    }
}
