using data_access;
using business_logic;
using Amazon_Back_End;
using data_access.data;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using Amazon_Back_End.Services;
using business_logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using data_access.data.Database;
using business_logic.Services;
using Hangfire;
namespace Amazon_Back_End
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

            builder.Services.AddDbContext<AmazonDbContext>(options =>
                    options.UseNpgsql(ConnectionString));

            builder.Services.AddScoped<IImageHulk, ImageHulk>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddIdentity();
            builder.Services.AddRepositories();
            builder.Services.AddAutoMapper();
            builder.Services.AddFluentValidators();
            builder.Services.AddCustomServices();
            builder.Services.AddScoped<ICartService, CartService>();

            builder.Services.AddHangfire(ConnectionString);

            var app = builder.Build();

            var dirImage = builder.Configuration["ImageFolder"] ?? "uploading";
            var dirPath = Path.Combine(Directory.GetCurrentDirectory(), dirImage);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(dirPath),
                RequestPath = "/images"
            });


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseAuthorization();

            app.UseHangfireDashboard("/dash");
            JobConfigurator.AddJobs();


            app.MapControllers();

            app.Run();
        }
    }
}
