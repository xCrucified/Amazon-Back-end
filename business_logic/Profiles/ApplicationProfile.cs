using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile(IFileService fileService)
        {
            CreateMap<Product, ProductDto>();
            
            CreateMap<ProductDto, Product>()
                .ForMember(x => x.ProductImages, opt =>
                    opt.MapFrom(x => x.images == null ?
                        new List<string>() : x.images.Select(pi => pi.Image).ToList()));

            CreateMap<CreateProductModel, Product>()
                 .ForMember(x => x.ProductImages, opt => opt.Ignore());
            CreateMap<EditProductModel, Product>();


            CreateMap<ProductProperties, ProductPropertiesDto>();
            CreateMap<ProductPropertiesDto, ProductProperties>();
            CreateMap<CreateProductPropertiesModel, ProductProperties>();


            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CreateCategoryModel, Category>();
            CreateMap<EditCategoryModel, Category>();

            CreateMap<ProductImage, ProductPropertiesDto>();
            CreateMap<ProductPropertiesDto, ProductProperties>();

            CreateMap<Subcategory, SubcategoryDto>();
            CreateMap<SubcategoryDto, Subcategory>();
            CreateMap<CreateSubcategoryModel, Subcategory>();
            CreateMap<EditSubcategoryModel, Subcategory>();

            CreateMap<ReviewDto, Review>();
            CreateMap<Review, ReviewDto>();
            CreateMap<CreateReviewModel, Review>();

            CreateMap<OrderDto, Order>();
            CreateMap<RegisterModel, User>();
        }
    }
}
