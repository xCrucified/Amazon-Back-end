using BLL.DTOs;
using BLL.DTOs.Wishlist;

namespace BLL.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistDto>> GetAllWishlistItems();
        Task<WishlistDto> GetWishlistById(string userId);
        Task Create(CreateWishlistModel itemDto);
        Task Edit(EditWishlistModel model);
        Task Delete(int id);
    }
}
