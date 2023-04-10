using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using PongServer.Application.Dtos.V1.Auth;
using PongServer.Application.Dtos.V1.Hosts;
using PongServer.Application.Dtos.V1.Users;
using PongServer.Application.Services.Auth;
using PongServer.Application.Services.Hosts;
using PongServer.Application.Services.PlayerScores;
using PongServer.Application.Services.UserContext;
using PongServer.Application.Services.Users;
using PongServer.Application.Validators.Auth;
using PongServer.Application.Validators.Filters;
using PongServer.Application.Validators.Hosts;
using PongServer.Application.Validators.Users;
using PongServer.Domain.Utils;

namespace PongServer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();

            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();
            services.AddScoped<IValidator<LoginUserDto>, LoginUserValidator>();
            services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordValidator>();
            services.AddScoped<IValidator<QueryFilters>, QueryFiltersValidator>();
            services.AddScoped<IValidator<CreateHostDto>, CreateHostValidator>();

            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IHostService, HostService>();
            services.AddScoped<IPlayerScoreService, PlayerScoreService>();

            return services;
        }
    }
}
