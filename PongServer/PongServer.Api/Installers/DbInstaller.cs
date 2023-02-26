using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PongServer.Infrastructure.Data;

namespace PongServer.Api.Installers
{
    internal class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<PongDataContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("DataConnection")));
        }
    }
}
