using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using PongServer.Api.BackgroundJobs;
using System.Reflection;

namespace PongServer.Api.Installers
{
    public class HangfireInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHangfire(cfg => cfg
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(Configuration.GetConnectionString("DataConnection")));
            services.AddHangfireServer();
            RegisterBackgroundJobsInAssembly(services.BuildServiceProvider());
        }

        private void RegisterBackgroundJobsInAssembly(IServiceProvider serviceProvider)
        {
            var backgroundJobs = typeof(HangfireInstaller).Assembly.GetTypes()
                .Where(x => typeof(IBackgroundJob).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IBackgroundJob>()
                .ToList();
            var recurringJobManager = serviceProvider.GetRequiredService(typeof(IRecurringJobManager)) as IRecurringJobManager;
            backgroundJobs.ForEach(job => job.RegisterJob(recurringJobManager));
        }
    }
}
