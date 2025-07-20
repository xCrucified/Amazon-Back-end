using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.DTOs;
using BLL.Entities;
using BLL.Interfaces;
using BLL.Specifications;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<ProductImage> _productImageRepo;
        private readonly IImageHulk _imageHulk;
        private readonly IWebHostEnvironment _environment;
        private readonly string imageFolder = "Images";

        public ProductService(IMapper _mapper, IRepository<Product> _productRepo, IImageHulk _imageHulk, IRepository<ProductImage> _productImageRepo, IWebHostEnvironment _environment)
        {
            this._mapper = _mapper;
            this._productRepo = _productRepo;
            this._imageHulk = _imageHulk;
            this._productImageRepo = _productImageRepo;
            this._environment = _environment;
        }

        public async Task Create(CreateProductModel productModel)
        {
            try
            {
                var productToInsert = _mapper.Map<Product>(productModel);

                await _productRepo.InsertAsync(productToInsert);
                await _productRepo.SaveChangesAsync();

                if (productModel.Images != null && productModel.Images.Any())
                {
                    foreach (var image in productModel.Images)
                    {
                        var imageName = await _imageHulk.Save(image);
                        var imageProduct = new ProductImage
                        {
                            Image = imageName,
                            ProductId = productToInsert.Id
                        };
                        await _productImageRepo.InsertAsync(imageProduct);
                    }
                    await _productImageRepo.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при створенні продукту: {ex.Message}");
                throw;
            }
        }

        public async Task Delete(int id)
        {
            var productToDelete = await _productRepo.GetItemBySpec(new ProductSpecs.ById(id));

            if (productToDelete == null)
                throw new HttpException(Errors.ItemNotFound, HttpStatusCode.NotFound);

            await _productRepo.DeleteAsync(productToDelete.Id);
            await _productRepo.SaveChangesAsync();
        }

        public async Task Edit(EditProductModel productEdit)
        {
            var existingProduct = await _productRepo.GetItemBySpec(new ProductSpecs.ById(productEdit.Id));

            if (existingProduct == null)
                throw new HttpException(Errors.ItemNotFound, HttpStatusCode.NotFound);

            _mapper.Map(productEdit, existingProduct);

            await _productRepo.UpdateAsync(existingProduct);
            await _productRepo.SaveChangesAsync();
        }

        public async Task<ProductDto> Get(int id)
        {
            if (id <= 0)
                throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var product = await _productRepo.GetItemBySpec(new ProductSpecs.ById(id));
            if (product == null)
                throw new HttpException(Errors.ItemNotFound, HttpStatusCode.NotFound);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> Get(IEnumerable<int> ids)
        {
            var products = await _productRepo.GetListBySpec(new ProductSpecs.ByIds(ids));
            return _mapper.Map<List<ProductDto>>(products);
        }

        public IQueryable<ProductDto> GetAll()
        {
            return _productRepo.GetQueryable()
                           .ProjectTo<ProductDto>(_mapper.ConfigurationProvider);
        }

        public IQueryable<ProductDto> GetBySubcategory(int subcategoryId)
        {
            return _productRepo.GetQueryable()
                               .Where(p => p.SubcategoryId == subcategoryId)
                               .ProjectTo<ProductDto>(_mapper.ConfigurationProvider);
        }
    }
}
