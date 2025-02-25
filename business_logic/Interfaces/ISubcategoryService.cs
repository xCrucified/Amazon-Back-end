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
        IEnumerable<SubcategoryDto> GetAll();
        Task<SubcategoryDto> GetById(int id);
        void Create(CreateSubcategoryModel model);
        Task Edit(EditSubcategoryModel model);
        Task Delete(int id);
    }
}
