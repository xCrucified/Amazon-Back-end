using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using data_access.data.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amazon_Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        public readonly IProductService productService;
        private readonly AmazonDbContext _amazonDbContext;
        private readonly IMapper _mapper;
        public readonly IImageHulk _imageHulk;

        public ProductController(IProductService productService,
            IMapper mapper,
            AmazonDbContext amazonDbContext,
            IImageHulk imageHulk)
        {
            this.productService = productService;
            _amazonDbContext = amazonDbContext;
            _mapper = mapper;
            _imageHulk = imageHulk;
        }

        [HttpGet("all")]
        public IActionResult GetAll() => Ok(this.productService.GetAll());

        [HttpGet("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            return Ok(await productService.Get(id));
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetProducts(
            int page = 1,
            int pageSize = 16,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? inStock = null,
            int? subcategoryId = null,
            string? search = null,
            float? minRating = null)
        {
            var query = productService.GetAll();

            // 🔍 Додаємо фільтрацію
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (inStock.HasValue && inStock.Value)
                query = query.Where(p => p.InStock > 0);

            if (subcategoryId.HasValue)
                query = query.Where(p => p.SubcategoryId == subcategoryId.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));

            if (minRating.HasValue)
                query = query.Where(p => p.Reviews.Any() && p.Reviews.Average(r => r.Rate) >= minRating.Value);

            // 📌 Підрахунок загальної кількості
            int totalCount = query.Count();

            // 🚀 Пагінація
            var products = query
                .OrderBy(p => p.Name) // або інший параметр сортування
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 📦 Відправляємо дані + метаінформацію
            return Ok(new
            {
                totalItems = totalCount,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                currentPage = page,
                pageSize = pageSize,
                products = products
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
            await productService.Edit(product);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles._ADMIN)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await productService.Delete(id);
            return Ok();
        }
    }
}
