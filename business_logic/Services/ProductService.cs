using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
        private readonly IRepository<ProductImage> productimageR;
        public readonly IImageHulk imageHulk;
        private readonly IWebHostEnvironment environment;
        private readonly string imageFolder = "Images";

        
        public ProductService(IMapper mapper, IRepository<Product> productR, IImageHulk hulk, IRepository<ProductImage> repository, IWebHostEnvironment environment)
        {
            this.mapper = mapper;
            this.productR = productR;
            this.imageHulk = hulk;
            this.productimageR = repository;
            this.environment = environment;
        }

        public async void Create(CreateProductModel productModel)
        {
            var ProductToInsert = mapper.Map<Product>(productModel);
            productR.Insert(ProductToInsert);
            productR.Save();
            if (productModel.Images != null)
            {
                foreach (var image in productModel.Images)
                {
                    var imageName = await imageHulk.Save(image);
                    var imageProduct = new ProductImagesDto
                    {
                        Image = imageName,
                        ProductId = ProductToInsert.Id
                    };
                    productimageR.Insert(mapper.Map<ProductImage>(imageProduct));
                }
            }
            productimageR.Save();
        }

        public async Task Delete(int ID)
        {
            if (await Get(ID) == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var product = mapper.Map<ProductDto>(Get(ID));

            productR.Delete(ID);
            productR.Save();
        }

        public async Task Edit(EditProductModel productEdit)
        {
            productR.Update(mapper.Map<Product>(productEdit));
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
