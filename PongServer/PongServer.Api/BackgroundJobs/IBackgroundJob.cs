using Hangfire;

namespace PongServer.Api.BackgroundJobs
{
    public interface IBackgroundJob
    {
        void RegisterJob(IRecurringJobManager recurringJobManager);
    }
}
