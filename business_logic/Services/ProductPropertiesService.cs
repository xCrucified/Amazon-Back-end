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
        public void Create(CreateProductPropertiesModel model)
        {
            var productProperties = mapper.Map<ProductProperties>(model);
            _productPropertiesRepository.Insert(productProperties);
            _productPropertiesRepository.Save();
        }

        public async Task Delete(int id, IEnumerable<int> propertyIds)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);
            var productProperties = await _productPropertiesRepository.GetItemBySpec(new ProductPropertiesSpecs.ByProduct(id, propertyIds));
        }


        public async Task Edit(EditProductPropertiesModel model)
        {
            _productPropertiesRepository.Update(mapper.Map<ProductProperties>(model));
            _productPropertiesRepository.Save();
        }

        public async Task<IEnumerable<ProductPropertiesDto>> GetAll()
        {
            return mapper.Map<IEnumerable<ProductPropertiesDto>>(_productPropertiesRepository.GetListBySpec(new ProductPropertiesSpecs.All()));
        }

        public async Task<IEnumerable<ProductPropertiesDto>> GetAllByProduct(int product)
        {
            return mapper.Map<IEnumerable<ProductPropertiesDto>>(_productPropertiesRepository.GetListBySpec(new ProductPropertiesSpecs.AllByProduct(product)));
        }

        public async Task<ProductPropertiesDto> GetById(int id)
        {
            return mapper.Map<ProductPropertiesDto>(await _productPropertiesRepository.GetItemBySpec(new ProductPropertiesSpecs.ById(id)));
        }

        public async Task<IEnumerable<ProductPropertiesDto>> GetByProduct(int productId, IEnumerable<int> propertyIds)
        {
            return mapper.Map<IEnumerable<ProductPropertiesDto>>(_productPropertiesRepository.GetListBySpec(new ProductPropertiesSpecs.ByProduct(productId, propertyIds)));
        }
    }
}
