using AutoMapper;
using business_logic.DTOs;
using business_logic.DTOs.PageRequest;
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

        [HttpPost("filtered")]
        public async Task<IActionResult> GetProducts([FromForm] ProductFilterRequestDto request)
        {
            if (request == null)
                return BadRequest(new { error = "Invalid request data" });

            var query = productService.GetAll();
            if (request.minPrice.HasValue)
                query = query.Where(p => p.Price >= request.minPrice.Value);

            if (request.maxPrice.HasValue)
                query = query.Where(p => p.Price <= request.maxPrice.Value);

            if (request.inStock.HasValue)
                query = query.Where(p => p.InStock > 0 == request.inStock.Value);

            if (request.categoryId.HasValue)
                query = query.Where(p => p.CategoryId == request.categoryId.Value);

            if (request.subcategoryId.HasValue)
                query = query.Where(p => p.SubcategoryId == request.subcategoryId.Value);

            if (!string.IsNullOrEmpty(request.search))
                query = query.Where(p => p.Name.Contains(request.search) || p.Description.Contains(request.search));

            if (request.minRating.HasValue)
                query = query.Where(p => p.Reviews.Any() && p.Reviews.Average(r => r.Rate) >= request.minRating.Value);

            int totalCount = query.Count();

            var products = query
                .OrderBy(p => p.Name)
                .Skip((request.page - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToList();

            return Ok(new
            {
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.pageSize),
                CurrentPage = request.page,
                PageSize = request.pageSize,
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

        [HttpGet("{id:int}/products")]
        public async Task<IActionResult> GetProductsBySubcategory([FromRoute] int id)
        {
            var products = productService.GetBySubcategory(id);
            return Ok(products);
        }
    }
}
