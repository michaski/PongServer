using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
                    opt.SignIn.RequireConfirmedEmail = true;
                    opt.Password.RequireDigit = true;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireUppercase = true;
                    opt.Password.RequireNonAlphanumeric = true;
                    opt.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<PongDataContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = "Bearer";
                    opt.DefaultScheme = "Bearer";
                    opt.DefaultChallengeScheme = "Bearer";
                })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Authentication:JwtSecretKey"])),
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidAudience = Configuration["Authentication:JwtIssuer"],
                        ValidIssuer = Configuration["Authentication:JwtIssuer"]
                    };
                    opt.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async ctx =>
                        {
                            var userManager = ctx.HttpContext.RequestServices
                                .GetRequiredService<UserManager<IdentityUser>>();
                            var user = await userManager.FindByIdAsync(
                                ctx.Principal.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
                            if (user is not null && !user.EmailConfirmed)
                            {
                                ctx.Fail("Account is not activated. Please confirm your email.");
                            }
                        }
                    };
                });
        }
    }
}
