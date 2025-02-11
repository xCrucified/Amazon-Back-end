using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class ProductPropertiesService : IProductPropertiesService
    {
        private readonly IRepository<ProductProperties> _productPropertiesRepository;
        private readonly IMapper mapper;
        public void CreateProductProperties(CreateProductPropertiesModel model)
        {
            var productProperties = mapper.Map<ProductProperties>(model);
            _productPropertiesRepository.Insert(productProperties);
            _productPropertiesRepository.Save();
        }

        public async Task DeleteProductProperties(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);
            var productProperties = await _productPropertiesRepository.GetItemBySpec(new ProductPropertiesSpecs.ByProduct(id));

        }

        public Task EditProductProperties(EditProductPropertiesModel model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductPropertiesDto> GetAllByProduct(int product)
        {
            throw new NotImplementedException();
        }
    }
}
