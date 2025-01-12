
using business_logic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryDto>> Get(IEnumerable<int> ids);
        public Task<CategoryDto> Get(int id);
        public IEnumerable<CategoryDto> GetAll();
        public void Create(CreateCategoryModel categoryModel);
        public Task Delete(int id);
        public Task Edit(EditCategoryModel categoryDto);
    }
}
