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

        public async Task DeleteProductProperties(int id, IEnumerable<int> propertyIds)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);
            var productProperties = await _productPropertiesRepository.GetItemBySpec(new ProductPropertiesSpecs.ByProduct(id, propertyIds));
        }


        public Task EditProductProperties(EditProductPropertiesModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductPropertiesDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductPropertiesDto>> GetAllByProduct(int product)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductPropertiesDto> GetById(int id)
        {
            return mapper.Map<ProductPropertiesDto>(await _productPropertiesRepository.GetItemBySpec(new ProductPropertiesSpecs.ById(id)));
        }

        public Task<IEnumerable<ProductPropertiesDto>> GetByProduct(int productId)
        {
            throw new NotImplementedException();
        }

    }
}
