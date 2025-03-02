using System.Text.Json;
using business_logic.Interfaces;
using Hangfire;


namespace Amazon_Back_End
{
    public class JobConfigurator
    {
        public static void AddJobs(){RemoveExpiredTokensJob();}
        public static void RemoveExpiredTokensJob()
        {
            RecurringJob.AddOrUpdate<IAccountService>(
                nameof(RemoveExpiredTokensJob),
                (service) => service.RemoveExpiredRefreshTokens(),
                Cron.Weekly(DayOfWeek.Monday, 3));
        }
    }
}
