using Hangfire;
using Hangfire.PostgreSql;

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
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowLocalhost");

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
