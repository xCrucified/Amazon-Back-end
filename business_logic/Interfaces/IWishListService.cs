using BLL.DTOs;
using BLL.DTOs.Wishlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistDto>> GetAllWishlistItems();
        Task<WishlistDto> GetWishlistById(string id);
        Task Create(CreateWishlistModel itemDto);
        Task Edit(EditWishlistModel model);
        Task Delete(int id);
    }
}
