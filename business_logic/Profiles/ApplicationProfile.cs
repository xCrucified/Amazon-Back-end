using AutoMapper;
using business_logic.DTOs;
using business_logic.DTOs.Wishlist;
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
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages));

            CreateMap<ProductDto, Product>()
                .ForMember(x => x.ProductImages, opt =>
                    opt.MapFrom(x => x.Images == null ?
                        new List<string>() : x.Images.Select(pi => pi.Image).ToList()));

            CreateMap<ProductImage, ProductImageDto>();

            CreateMap<CreateProductModel, Product>()
                 .ForMember(x => x.ProductImages, opt => opt.Ignore());
            CreateMap<EditProductModel, Product>();


            CreateMap<Subcategory, SubcategoryDto>().ReverseMap(); // might cause an issue
            CreateMap<CreateSubcategoryModel, Subcategory>();
            CreateMap<EditSubcategoryModel, Subcategory>();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryModel, Category>();
            CreateMap<EditCategoryModel, Category>();


            CreateMap<ReviewDto, Review>().ReverseMap();
            CreateMap<CreateReviewModel, Review>();

            CreateMap<OrderDto, Order>();
            CreateMap<RegisterModel, User>();


            CreateMap<Wishlist, WishlistDto>().ReverseMap();
            CreateMap<CreateWishlistModel, Wishlist>();
            CreateMap<EditWishlistModel, Wishlist>();
        }
    }
}
