using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Services;
using data_access.data.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using static Amazon_Back_End.Helpers.SeedExtension;

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
            return  Ok(await productService.Get(id));
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
            //productService.Create(createProductModel);
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
