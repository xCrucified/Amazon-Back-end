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
        public async Task<IActionResult> GetWishList(string userId)
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

        [HttpDelete("{userId}/{itemid}")]
        public async Task<IActionResult> RemoveFromWishList(string userId, int itemid)
        {
            await _wishListService.RemoveWishlistItem(userId, itemid);
            return Ok("Item removed from wishlist.");
        }
    }
}
