using business_logic.DTOs;
using business_logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Interfaces
{
    public interface IReviewService
    {
        public IEnumerable<ReviewDto> GetAll();
        public Task<ReviewDto> Get(int id);
        public void Create(CreateReviewModel createReviewModel);
        public Task Delete(int id);
    }
}
