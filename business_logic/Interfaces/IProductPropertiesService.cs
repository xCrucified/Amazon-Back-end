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
        public Task<IEnumerable<ProductPropertiesDto>> GetByProduct(int productId, IEnumerable<int> propertyIds);
        public Task<IEnumerable<ProductPropertiesDto>> GetAllByProduct(int product);
        public Task<ProductPropertiesDto> GetById(int id);
        public Task<IEnumerable<ProductPropertiesDto>> GetAll();
        public void Create(CreateProductPropertiesModel model);
        public Task Delete(int id, IEnumerable<int> propertyIds);
        public Task Edit(EditProductPropertiesModel model);
    }
}
