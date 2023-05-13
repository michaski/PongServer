using Hangfire;
using PongServer.Application.Services.Games;

namespace PongServer.Api.BackgroundJobs.Games
{
    public class DeleteStaleGamesJob : IBackgroundJob
    {

        public void RegisterJob(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.AddOrUpdate<IGameService>(
                "removestalegames",
                service => service.DeleteStaleGamesAsync(),
                Cron.Hourly);
        }
    }
}
