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
    }
}
