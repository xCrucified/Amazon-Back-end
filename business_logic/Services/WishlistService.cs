using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IRepository<WishListItem> wishlistitemR;
        private readonly IMapper _mapper;

        public WishlistService(IRepository<WishListItem> R, IMapper mapp)
        {
            this.wishlistitemR = R;
            this._mapper = mapp;
        }

        public async Task<IEnumerable<WishlistItemDto>> GetAllWishlistItems()
        {
            var wishlistitems = wishlistitemR.GetAll() ?? Enumerable.Empty<WishListItem>();
            return _mapper.Map<List<WishlistItemDto>>(wishlistitems);
        }

        public async Task<WishlistItemDto> GetWishlistItemById(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var wishllstitem = await wishlistitemR.GetItemBySpec(new WishlistSpecs.ById(id));
            if (wishllstitem == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            return _mapper.Map<WishlistItemDto>(wishllstitem);
        }

        public async Task AddWishlistItem(WishlistItemDto itemDto)
        {
            var item = _mapper.Map<WishListItem>(itemDto);
            wishlistitemR.Insert(item);
            wishlistitemR.Save();
        }

        public async Task RemoveWishlistItem(string userId, int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var wishlistitem = await wishlistitemR.GetItemBySpec(new WishlistSpecs.ById(id));
            if (wishlistitem == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            wishlistitemR.Delete(id);
            wishlistitemR.Save();
        }
    }
}
