using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface ICartService
    {
        Task ClearCart(string userId);
        Task Delete(int id);
        Task Create(CreateCartItemModel cartitem);
        Task Edit(EditCartItemModel model);
        Task<CartItemDto> Get(int id);
        Task<IEnumerable<CartItemDto>> GetAll();
        Task<IEnumerable<CartItemDto>> GetByUser(string id);
    }
}
