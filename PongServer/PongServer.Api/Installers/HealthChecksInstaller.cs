using PongServer.Infrastructure.Data;

namespace PongServer.Api.Installers
{
    public class HealthChecksInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<PongDataContext>();
        }
    }
}
