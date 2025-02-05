using data_access;
using business_logic;
using Amazon_Back_End;
using data_access.data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using Amazon_Back_End.Services;
using business_logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using data_access.data.Database;
using business_logic.Services;
namespace Amazon_Back_End
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;

            builder.Services.AddDbContext<AmazonDbContext>(options =>
                    options.UseNpgsql(connStr));

            builder.Services.AddScoped<IImageHulk, ImageHulk>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext(connStr);
            builder.Services.AddIdentity();
            builder.Services.AddRepositories();
            builder.Services.AddAutoMapper();
            builder.Services.AddFluentValidators();
            builder.Services.AddCustomServices();
            builder.Services.AddScoped<ICartService, CartService>();

            var app = builder.Build();

            //var dirImage = builder.Configuration["ImageFolder"] ?? "uploading";
            //var dirPath = Path.Combine(Directory.GetCurrentDirectory(), dirImage);
            //if (!Directory.Exists(dirPath))
            //    Directory.CreateDirectory(dirPath);

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(dirPath),
            //    RequestPath = "/images"
            //});


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
