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
        Task<IEnumerable<WishlistItemDto>> GetAllWishlistItems();
        Task<WishlistItemDto> GetWishlistItemById(int id);
        Task AddWishlistItem(WishlistItemDto itemDto);
        Task RemoveWishlistItem(string userId, int id);
    }
}
