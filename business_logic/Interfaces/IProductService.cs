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
        public Task<IEnumerable<ProductDto>> Get(IEnumerable<int> ids);
        public Task<ProductDto> Get(int id);
        IEnumerable<ProductDto> GetAll();
        public Task Create(CreateProductModel productModel);
        public Task Delete(int id);
        public Task Edit(EditProductModel productEdit);
    }
}
