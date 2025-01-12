using business_logic.DTOs;
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
        public IEnumerable<ProductDto> GetAll();
        public void Create(CreateProductModel productModel);
        public Task Delete(int id);
        public Task Edit(EditProductModel productEdit);
    }
}
