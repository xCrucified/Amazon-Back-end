using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface ISubcategoryService
    {
        IEnumerable<SubcategoryDto> GetAll();
        Task<SubcategoryDto> Get(int id);
        Task<IEnumerable<SubcategoryDto>> GetSubcategoriesByCategoryNameAsync(string categoryName);
        Task<IEnumerable<SubcategoryDto>> GetSubcategoriesByCategoryAsync(int id);
        void Create(CreateSubcategoryModel model);
        Task Edit(EditSubcategoryModel model);
        Task Delete(int id);
    }
}
