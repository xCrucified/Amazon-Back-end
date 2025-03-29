using business_logic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Interfaces
{
    public interface ICartService
    {
        Task ClearCart(string userId);
        Task Delete(int id);
        void Create(CreateCartItemModel cartitem);
        Task Edit(EditCartItemModel model);
        Task<CartItemDto> Get(int id);
        IEnumerable<CartItemDto> GetAll();
        IEnumerable<CartItemDto> GetByUser(string id);
    }
}
