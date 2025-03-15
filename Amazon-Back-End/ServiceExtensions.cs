using business_logic.Helpers;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Amazon_Back_End
{
    public static class ServiceExtensions
    {
        public static void AddHangfire(this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(config =>
            {
                config.UsePostgreSqlStorage(connectionString);
            });
            services.AddHangfireServer();
        }

        public static void AddJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOpts = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOpts.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpts.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}
