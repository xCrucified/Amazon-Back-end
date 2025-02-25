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


            CreateMap<ProductProperties, ProductPropertiesDto>().ReverseMap();
            CreateMap<CreateProductPropertiesModel, ProductProperties>();

            CreateMap<Subcategory, SubcategoryDto>().ReverseMap();
            CreateMap<CreateSubcategoryModel, Subcategory>();
            CreateMap<EditSubcategoryModel, Subcategory>();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryModel, Category>();
            CreateMap<EditCategoryModel, Category>();

            CreateMap<ProductProperties, ProductPropertiesDto>().ReverseMap();
            CreateMap<CreateProductPropertiesModel, ProductProperties>();
            CreateMap<EditProductPropertiesModel, ProductProperties>();

            CreateMap<Subcategory, SubcategoryDto>().ReverseMap();
            CreateMap<CreateSubcategoryModel, Subcategory>();
            CreateMap<EditSubcategoryModel, Subcategory>();

            CreateMap<ReviewDto, Review>().ReverseMap();
            CreateMap<CreateReviewModel, Review>();

            CreateMap<OrderDto, Order>();
            CreateMap<RegisterModel, User>();
        }
    }
}
