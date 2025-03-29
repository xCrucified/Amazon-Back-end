using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<CartItem> cartR;
        private readonly IMapper mapper;

        public CartService(IRepository<CartItem> cartR, IMapper mapper)
        {
            this.cartR = cartR;
            this.mapper = mapper;
        }

        public void Create(CreateCartItemModel cartitem)
        {
            cartR.Insert(mapper.Map<CartItem>(cartitem));
            cartR.Save();
        }

        public async Task Delete(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var cartitem = await Get(id);
            var cartitemDto = mapper.Map<CartItemDto>(cartitem);

            cartR.Delete(id);
            cartR.Save();
        }

        public async Task ClearCart(string userId)
        {
            var cartitems = cartR.GetListBySpec(new CartItemSpecs.ByUserId(userId)).Result;

            foreach (var item in cartitems)
            {
                cartR.Delete(item.Id);
            }

            cartR.Save();
        }

        public async Task<CartItemDto> Get(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var product = await cartR.GetItemBySpec(new CartItemSpecs.ById(id));
            if (product == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            return mapper.Map<CartItemDto>(product);
        }

        public IEnumerable<CartItemDto> GetByUser(string id)
        {
            var carts = cartR.GetListBySpec(new CartItemSpecs.ByUserId(id)).Result;

            return mapper.Map<List<CartItemDto>>(carts);
        }

        public IEnumerable<CartItemDto> GetAll()
        {
            var carts = cartR.GetListBySpec(new CartItemSpecs.All()).Result;

            return mapper.Map<List<CartItemDto>>(carts);
        }

        public async Task Edit(EditCartItemModel model)
        {
            cartR.Update(mapper.Map<CartItem>(model));
            cartR.Save();
        }
    }
}
