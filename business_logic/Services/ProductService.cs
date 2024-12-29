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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace business_logic.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Product> productR; 

        public ProductService(IMapper mapper, IRepository<Product> productR)
        {
            this.mapper = mapper;
            this.productR = productR;
        }


        public void Create(CreateProductModel productModel)
        {
            productR.Insert(mapper.Map<Product>(productModel));
            productR.Save();
        }

        public async Task Delete(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var pr = productR.GetById(id);

            productR.Delete(id);
            productR.Save();
        }

        public async Task Edit(ProductDto productDto)
        {
            productR.Update(mapper.Map<Product>(productR));
            productR.Save();
        }


        public async Task<ProductDto> Get(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var product = await productR.GetItemBySpec(new ProductSpecs.ById(id));
            if(product == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            return mapper.Map<ProductDto>(product);
        }

        public IEnumerable<ProductDto> GetAll()
        {
            return mapper.Map<List<ProductDto>>(productR.GetAll());
        }

        async Task<IEnumerable<ProductDto>> IProductService.Get(IEnumerable<int> ids)
        {
            return mapper.Map<List<ProductDto>>(await productR.GetListBySpec(new ProductSpecs.ByIds(ids)));
        }
    }
}
