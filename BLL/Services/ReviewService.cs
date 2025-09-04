using AutoMapper;
using BLL.DTOs;
using BLL.Entities;
using BLL.Interfaces;
using BLL.Specifications;
using System.Net;

namespace BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Review> _reviewRepo;

        public ReviewService(IMapper mapper, IRepository<Review> repository)
        {
            _mapper = mapper;
            _reviewRepo = repository;
        }

        public async Task Create(CreateReviewModel createReviewModel)
        {
            var reviewToInsert = _mapper.Map<Review>(createReviewModel);
            await _reviewRepo.InsertAsync(reviewToInsert);
            await _reviewRepo.SaveChangesAsync();
        }

        public async Task<ReviewDto> Get(int id)
        {
            if (id <= 0)
                throw new HttpException("Review not found.", HttpStatusCode.BadRequest);

            var review = await _reviewRepo.GetItemBySpec(new ReviewSpecs.ById(id));
            if (review == null)
                throw new HttpException("Review not found.", HttpStatusCode.NotFound);

            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetAll()
        {
            var reviews = await _reviewRepo.GetListBySpec(new ReviewSpecs.All());
            return _mapper.Map<List<ReviewDto>>(reviews);
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new HttpException("Review not found.", HttpStatusCode.BadRequest);

            var reviewToDelete = await _reviewRepo.GetItemBySpec(new ReviewSpecs.ById(id));

            if (reviewToDelete == null)
                throw new HttpException("Review not found.", HttpStatusCode.NotFound);

            await _reviewRepo.DeleteAsync(reviewToDelete);
            await _reviewRepo.SaveChangesAsync();
        }
    }
}