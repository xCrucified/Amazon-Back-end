using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryDto>> Get(IEnumerable<int> ids);
        public Task<CategoryDto> Get(int id);
        public Task<IEnumerable<CategoryDto>> GetAll();
        public Task Create(CreateCategoryModel categoryModel);
        public Task Delete(int id);
        public Task Edit(EditCategoryModel categoryDto);
    }
}
