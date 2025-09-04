using AutoMapper;
using BLL.DTOs;
using BLL.DTOs.Wishlist;
using BLL.Entities;
using BLL.Interfaces;
using BLL.Specifications;
using System.Net;

namespace BLL.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IRepository<Wishlist> _wishlistRepo;
        private readonly IMapper _mapper;

        public WishlistService(IRepository<Wishlist> wishlistRepo, IMapper mapper)
        {
            _wishlistRepo = wishlistRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WishlistDto>> GetAllWishlistItems()
        {
            var wishlistItems = await _wishlistRepo.GetListBySpec(new WishlistSpecs.All());
            return _mapper.Map<List<WishlistDto>>(wishlistItems);
        }

        public async Task<WishlistDto> GetWishlistById(string userId)
        {
            var wishlistitem = await _wishlistRepo.GetItemBySpec(new WishlistSpecs.ByUserId(userId));
            if (wishlistitem == null)
                throw new HttpException("Wishlist item not found for this user.", HttpStatusCode.NotFound);

            return _mapper.Map<WishlistDto>(wishlistitem);
        }

        public async Task Create(CreateWishlistModel itemDto)
        {
            var item = _mapper.Map<Wishlist>(itemDto);
            await _wishlistRepo.InsertAsync(item);
            await _wishlistRepo.SaveChangesAsync();
        }

        public async Task Edit(EditWishlistModel model)
        {
            var existingWishlistItem = await _wishlistRepo.GetItemBySpec(new WishlistSpecs.ById(model.Id));

            if (existingWishlistItem == null)
                throw new HttpException("Wishlist item not found.", HttpStatusCode.NotFound);

            _mapper.Map(model, existingWishlistItem);

            await _wishlistRepo.UpdateAsync(existingWishlistItem);
            await _wishlistRepo.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new HttpException("Wishlist item not found.", HttpStatusCode.BadRequest);

            var wishlistItemToDelete = await _wishlistRepo.GetItemBySpec(new WishlistSpecs.ById(id));

            if (wishlistItemToDelete == null)
                throw new HttpException("Wishlist item not found.", HttpStatusCode.NotFound);

            await _wishlistRepo.DeleteAsync(wishlistItemToDelete);
            await _wishlistRepo.SaveChangesAsync();
        }
    }
}