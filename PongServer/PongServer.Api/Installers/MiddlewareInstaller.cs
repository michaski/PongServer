using PongServer.Api.Middleware;

namespace PongServer.Api.Installers
{
    public class MiddlewareInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<ErrorHandlingMiddleware>();
        }
    }
}
