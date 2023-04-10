using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PongServer.Domain.Interfaces;
using PongServer.Infrastructure.Repositories;

namespace PongServer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IHostRepository, HostRepository>();
            services.AddScoped<IPlayerScoreRepository, PlayerScoreRepository>();

            return services;
        }
    }
}
