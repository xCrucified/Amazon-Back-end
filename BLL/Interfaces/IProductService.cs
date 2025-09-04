using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> Get(IEnumerable<int> ids);
        Task<ProductDto> Get(int id);

        IQueryable<ProductDto> GetAll();

        Task Create(CreateProductModel productModel);
        Task Delete(int id);
        Task Edit(EditProductModel productEdit);

        IQueryable<ProductDto> GetBySubcategory(int subcategoryId);
    }
}
