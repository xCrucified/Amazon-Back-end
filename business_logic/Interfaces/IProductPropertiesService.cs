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
        public IEnumerable<ProductPropertiesDto> GetAllByProduct(int product);
        public void CreateProductProperties(CreateProductPropertiesModel model);
        public Task DeleteProductProperties(int id);
        public Task EditProductProperties(EditProductPropertiesModel model);
    }
}
