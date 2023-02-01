using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Infrastructure.Data;

namespace PongServer.Api.Installers
{
    internal class IdentityInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<IdentityContext>(opt =>
            {
                opt.UseNpgsql(Configuration["ConnectionStrings:IdentityConnection"]);
            });
            services.AddIdentity<IdentityUser, IdentityRole>(opt =>
                    opt.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<IdentityContext>();
        }
    }
}
