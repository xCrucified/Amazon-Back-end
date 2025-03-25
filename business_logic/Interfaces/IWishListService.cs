using business_logic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishListItemDto>> GetAllWishlistItems();
        Task<WishListItemDto> GetWishlistItemById(int id);
        Task AddWishlistItem(WishListItemDto itemDto);
        Task RemoveWishlistItem(string userId, int id);
    }
}
