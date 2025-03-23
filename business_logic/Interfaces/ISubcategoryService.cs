using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using business_logic.DTOs;
using business_logic.Entities;
using Microsoft.AspNetCore.Mvc;

namespace business_logic.Interfaces
{
    public interface ISubcategoryService
    {
        IEnumerable<SubcategoryDto> GetAll();
        Task<SubcategoryDto> GetById(int id);
        Task<IEnumerable<SubcategoryDto>> GetSubcategoriesByCategoryNameAsync(string categoryName);
        Task<IEnumerable<SubcategoryDto>> GetSubcategoriesByCategoryAsync(int id);
        void Create(CreateSubcategoryModel model);
        Task Edit(EditSubcategoryModel model);
        Task Delete(int id);
    }
}
