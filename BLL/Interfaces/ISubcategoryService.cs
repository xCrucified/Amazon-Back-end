using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<SubcategoryDto>> GetAll();
        Task<SubcategoryDto> Get(int id);
        Task<IEnumerable<SubcategoryDto>> GetSubcategoriesByCategoryNameAsync(string categoryName);
        Task<IEnumerable<SubcategoryDto>> GetSubcategoriesByCategoryAsync(int id);
        Task Create(CreateSubcategoryModel model);
        Task Edit(EditSubcategoryModel model);
        Task Delete(int id);
    }
}
