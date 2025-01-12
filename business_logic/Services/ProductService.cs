using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;
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
        //private readonly IFileService fileService;
        public ProductService(IMapper mapper, IRepository<Product> productR)
        {
            this.mapper = mapper;
            this.productR = productR;
        }


        public void Create(CreateProductModel productModel)
        {


            var p = mapper.Map<Product>(productModel);

            string root = Directory.GetCurrentDirectory();
            string name = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(productModel.Image.FileName);
            string fullName = name + extension;
            string imageFolder = "images";

            string imagePath = Path.Combine(imageFolder, fullName);
            Directory.CreateDirectory(Path.Combine(root, imageFolder));
            string imageFullPath = Path.Combine(root, imagePath);


            using (FileStream fs = new FileStream(imageFullPath, FileMode.Create))
            {
                if (productModel.Image != null)
                {
                    productModel.Image.CopyTo(fs);
                }
            }
            p.Image = fullName;
            productR.Insert(p);
            productR.Save();
        }

        public async Task Delete(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var pr = productR.GetById(id);

            productR.Delete(id);
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
