using BLL.DTOs;
using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IReviewService
    {
        public IEnumerable<ReviewDto> GetAll();
        public Task<ReviewDto> Get(int id);
        public void Create(CreateReviewModel createReviewModel);
        public Task Delete(int id);
    }
}
