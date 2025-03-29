using business_logic.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> Get(IEnumerable<int> ids);
        Task<ProductDto> Get(int id);
        IEnumerable<ProductDto> GetAll();
        Task Create(CreateProductModel productModel);
        Task Delete(int id);
        Task Edit(EditProductModel productEdit);
        IEnumerable<ProductDto> GetBySubcategory(int subcategoryId);
    }
}
