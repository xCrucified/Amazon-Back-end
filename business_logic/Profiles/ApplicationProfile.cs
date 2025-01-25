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
            
            CreateMap<ProductDto, Product>();
            
            CreateMap<CreateProductModel, Product>();

            CreateMap<EditProductModel, Product>();

            CreateMap<Category, CategoryDto>();
            
            CreateMap<CategoryDto, Category>();

            CreateMap<ReviewDto, Review>();
            CreateMap<Review, ReviewDto>();
            CreateMap<CreateReviewModel, Review>();

            CreateMap<OrderDto, Order>();
            
            CreateMap<CreateCategoryModel, Category>();
            CreateMap<EditCategoryModel, Category>();
            CreateMap<RegisterModel, User>();
        }
    }
}
