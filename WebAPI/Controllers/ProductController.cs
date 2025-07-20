using AutoMapper;
using BLL.DTOs;
using BLL.DTOs.PageRequest;
using BLL.Entities;
using BLL.Interfaces;
using DAL.data.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        public readonly IProductService _productService;
        private readonly AmazonDbContext _amazonDbContext;
        private readonly IMapper _mapper;
        public readonly IImageHulk _imageHulk;

        public ProductController(IProductService productService,
            IMapper mapper,
            AmazonDbContext amazonDbContext,
            IImageHulk imageHulk)
        {
            this._productService = productService;
            this._amazonDbContext = amazonDbContext;
            this._mapper = mapper;
            this._imageHulk = imageHulk;
        }

        [HttpGet("all")]
        public IActionResult GetAll() => Ok(this._productService.GetAll());

        [HttpGet("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            return Ok(await _productService.Get(id));
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] ProductFilterRequestDto request)
        {
            if (request == null)
                return BadRequest(new { error = "Некоректні вхідні дані запиту." });

            if (request.Page < 1)
                return BadRequest(new { error = "Номер сторінки не може бути меншим за 1." });

            if (request.PageSize < 1)
                return BadRequest(new { error = "Розмір сторінки не може бути меншим за 1." });

            var query = _productService.GetAll();

            if (request.MinPrice.HasValue)
                query = query.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= request.MaxPrice.Value);

            if (request.InStock.HasValue)
                query = query.Where(p => (p.InStock > 0) == request.InStock.Value);

            if (request.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == request.CategoryId.Value);

            if (request.SubcategoryId.HasValue)
                query = query.Where(p => p.SubcategoryId == request.SubcategoryId.Value);

            if (!string.IsNullOrEmpty(request.Search))
            {
                string searchTerm = request.Search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm) || p.Description.ToLower().Contains(searchTerm));
            }

            if (request.MinRating.HasValue)
                query = query.Where(p => p.Reviews.Any() && p.Reviews.Average(r => r.Rate) >= request.MinRating.Value);

            int totalCount = await query.CountAsync();

            var products = await query
                .OrderBy(p => p.Name)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                Products = products
            });
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Create([FromForm] CreateProductModel createProductModel)
        {
            try
            {
                var productToInsert = _mapper.Map<Product>(createProductModel);
                _amazonDbContext.Products.Add(productToInsert);
                _amazonDbContext.SaveChanges();

                foreach (var image in createProductModel.Images)
                {
                    var imageName = await _imageHulk.Save(image);
                    var imageProduct = new ProductImage
                    {
                        Image = imageName,
                        ProductId = productToInsert.Id
                    };
                    _amazonDbContext.ProductImages.Add(imageProduct);
                }
                _amazonDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Edit([FromForm] EditProductModel product)
        {
            await _productService.Edit(product);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _productService.Delete(id);
            return Ok();
        }

        [HttpGet("{id:int}/products")]
        public async Task<IActionResult> GetProductsBySubcategory([FromRoute] int id)
        {
            var products = _productService.GetBySubcategory(id);
            return Ok(products);
        }
    }
}
