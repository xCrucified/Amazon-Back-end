using AutoMapper;
using BLL.DTOs;
using BLL.Entities;
using BLL.Interfaces;
using BLL.Specifications;
using System.Net;

namespace BLL.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<CartItem> _cartRepo;
        private readonly IMapper _mapper;

        public CartService(IRepository<CartItem> cartRepo, IMapper mapper)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
        }

        public async Task Create(CreateCartItemModel cartitemModel)
        {
            var cartItemToInsert = _mapper.Map<CartItem>(cartitemModel);
            await _cartRepo.InsertAsync(cartItemToInsert);
            await _cartRepo.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new HttpException("Cart item not found.", HttpStatusCode.BadRequest);

            var cartItemToDelete = await _cartRepo.GetItemBySpec(new CartItemSpecs.ById(id));

            if (cartItemToDelete == null)
                throw new HttpException("Cart item not found.", HttpStatusCode.NotFound);

            await _cartRepo.DeleteAsync(cartItemToDelete);
            await _cartRepo.SaveChangesAsync();
        }

        public async Task ClearCart(string userId)
        {
            var cartItems = await _cartRepo.GetListBySpec(new CartItemSpecs.ByUserId(userId));

            if (cartItems != null && cartItems.Any())
            {
                foreach (var item in cartItems)
                {
                    await _cartRepo.DeleteAsync(item);
                }
                await _cartRepo.SaveChangesAsync();
            }
        }

        public async Task<CartItemDto> Get(int id)
        {
            if (id <= 0)
                throw new HttpException("Cart item not found.", HttpStatusCode.BadRequest);

            var cartItem = await _cartRepo.GetItemBySpec(new CartItemSpecs.ById(id));
            if (cartItem == null)
                throw new HttpException("Cart item not found.", HttpStatusCode.NotFound);

            return _mapper.Map<CartItemDto>(cartItem);
        }

        public async Task<IEnumerable<CartItemDto>> GetByUser(string userId)
        {
            var carts = await _cartRepo.GetListBySpec(new CartItemSpecs.ByUserId(userId));
            return _mapper.Map<List<CartItemDto>>(carts);
        }

        public async Task<IEnumerable<CartItemDto>> GetAll()
        {
            var carts = await _cartRepo.GetListBySpec(new CartItemSpecs.All());
            return _mapper.Map<List<CartItemDto>>(carts);
        }

        public async Task Edit(EditCartItemModel model)
        {
            var existingCartItem = await _cartRepo.GetItemBySpec(new CartItemSpecs.ById(model.Id));

            if (existingCartItem == null)
                throw new HttpException("Cart item not found.", HttpStatusCode.NotFound);

            _mapper.Map(model, existingCartItem);

            await _cartRepo.UpdateAsync(existingCartItem);
            await _cartRepo.SaveChangesAsync();
        }
    }
}