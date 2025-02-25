using AutoMapper;
using business_logic.Interfaces;
using business_logic.Profiles;
using business_logic.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic
{
    public static class ServiceExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile(provider.CreateScope().ServiceProvider.GetService<IFileService>()!));
            }).CreateMapper());
        }

        public static void AddFluentValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductPropertiesService, ProductPropertiesService>();
            services.AddScoped<ISubcategoryService, SubcategoryService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IImageHulk, ImageHulk>();
            services.AddScoped<IEmailSender, MailjetSender>();
        }
    }
}
