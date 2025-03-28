using AutoMapper;
using business_logic.DTOs;
using business_logic.DTOs.Wishlist;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;
using Microsoft.AspNetCore.Identity;
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
        private readonly IRepository<Wishlist> wishlistR;
        private readonly UserManager<User> userManager;
        private readonly IMapper _mapper;

        public WishlistService(IRepository<Wishlist> R, IMapper mapp)
        {
            this.wishlistR = R;
            this._mapper = mapp;
        }

        public async Task<IEnumerable<WishlistDto>> GetAllWishlistItems()
        {
            var wishlistitems = wishlistR.GetAll() ?? Enumerable.Empty<Wishlist>();
            return _mapper.Map<List<WishlistDto>>(wishlistitems);
        }

        public async Task<WishlistDto> GetWishlistById(string id)
        {
            var wishllstitem = await wishlistR.GetItemBySpec(new WishlistSpecs.ByUserId(id));
            if (wishllstitem == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            return _mapper.Map<WishlistDto>(wishllstitem);
        }

        public async Task<WishlistDto> GetWishlistById(int id)
        {
            var wishllstitem = await wishlistR.GetItemBySpec(new WishlistSpecs.ById(id));
            if (wishllstitem == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            return _mapper.Map<WishlistDto>(wishllstitem);
        }

        public async Task Create(CreateWishlistModel itemDto)
        {
            var item = _mapper.Map<Wishlist>(itemDto);
            wishlistR.Insert(item);
            wishlistR.Save();
        }

        public async Task Edit(EditWishlistModel model)
        {
            wishlistR.Update(_mapper.Map<Wishlist>(model));
            wishlistR.Save();
        }

        public async Task Delete(int id)
        {
            if (await GetWishlistById(id) == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var wishlist = await GetWishlistById(id);
            var wishlistDto = _mapper.Map<Wishlist>(wishlist);

            wishlistR.Delete(id);
            wishlistR.Save();
        }
    }
}
