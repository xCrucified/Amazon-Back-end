using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;

namespace business_logic.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Review> reviewR;

        public ReviewService(IMapper mapper, IRepository<Review> repository)
        {
            this.mapper = mapper;
            this.reviewR = repository;
        }

        public void Create(CreateReviewModel createReviewModel)
        {
            reviewR.Insert(mapper.Map<Review>(createReviewModel));
            reviewR.Save();
        }

        public async Task<ReviewDto> Get(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var product = await reviewR.GetItemBySpec(new ReviewSpecs.ById(id));
            if (product == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            return mapper.Map<ReviewDto>(product);
        }

        public IEnumerable<ReviewDto> GetAll()
        {
            return mapper.Map<List<ReviewDto>>(reviewR.GetAll());
        }

        public async Task Delete(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            reviewR.Delete(id);
            reviewR.Save();
        }
    }
}
