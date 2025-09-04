using BLL.Interfaces;
using Hangfire;

namespace WebAPI
{
    public class JobConfigurator
    {
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IAccountService _accountService;

        public JobConfigurator(IRecurringJobManager recurringJobManager, IAccountService accountService)
        {
            _recurringJobManager = recurringJobManager;
            _accountService = accountService;
        }

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
