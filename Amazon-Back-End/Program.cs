using Amazon_Back_End.Helpers;
using Amazon_Back_End.Services;
using business_logic;
using business_logic.Interfaces;
using business_logic.Services;
using data_access;
using data_access.data.Database;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/signin-google";
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // ��� ������ ����� ��������� HTTPS
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Key"]))
                };
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure HTTPS
                options.Cookie.SameSite = SameSiteMode.None;  // Required for OAuth
            });

            builder.Services.AddHttpContextAccessor();
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
            builder.Services.AddHttpClient<IGoogleAuthService, GoogleAuthService>();
            builder.Services.AddHealthChecks();

            builder.Services.AddHangfire(ConnectionString);

            var app = builder.Build();

            app.UseRouting();

            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:3000", "http://localhost:5000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.CreateRoles().Wait();
                scope.ServiceProvider.SeedAdmin().Wait();
            }

            var dirImage = builder.Configuration["ImageFolder"] ?? "uploading";
            var dirPath = Path.Combine(Directory.GetCurrentDirectory(), dirImage);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(dirPath),
                RequestPath = "/Images"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz");
                endpoints.MapControllers();
            });

            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHangfireDashboard("/dash");
            JobConfigurator.AddJobs();

            app.MapControllers();

            app.Run();
        }
    }
}
