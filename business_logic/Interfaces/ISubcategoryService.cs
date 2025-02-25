using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using business_logic.DTOs;

namespace business_logic.Interfaces
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<SubcategoryDto>> GetSubcategories();
        Task<SubcategoryDto> GetSubcategory(int id);
        Task<SubcategoryDto> CreateSubcategory(CreateSubcategoryModel model);
        Task<SubcategoryDto> EditSubcategory(EditSubcategoryModel model);
        Task DeleteSubcategory(int id);
    }
}
