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
            services.AddDbContext<PongDataContext>(opt =>
            {
                opt.UseNpgsql(Configuration["ConnectionStrings:DataConnection"]);
            });
            services.AddIdentity<IdentityUser, IdentityRole>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                    opt.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<PongDataContext>()
                .AddDefaultTokenProviders();
        }
    }
}
