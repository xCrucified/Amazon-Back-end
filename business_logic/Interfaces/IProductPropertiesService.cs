using business_logic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Interfaces
{
    public interface IProductPropertiesService
    {
        public Task<IEnumerable<ProductPropertiesDto>> GetByProduct(int productId);
        public Task<IEnumerable<ProductPropertiesDto>> GetAllByProduct(int product);
        public Task<ProductPropertiesDto> GetById(int id);
        public Task<IEnumerable<ProductPropertiesDto>> GetAll();
        public void CreateProductProperties(CreateProductPropertiesModel model);
        public Task DeleteProductProperties(int id, IEnumerable<int> propertyIds);
        public Task EditProductProperties(EditProductPropertiesModel model);
    }
}
