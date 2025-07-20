using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface IReviewService
    {
        public Task<IEnumerable<ReviewDto>> GetAll();
        public Task<ReviewDto> Get(int id);
        public Task Create(CreateReviewModel createReviewModel);
        public Task Delete(int id);
    }
}
