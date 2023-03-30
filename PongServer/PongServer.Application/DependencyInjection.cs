using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using PongServer.Application.Dtos.Auth;
using PongServer.Application.Services.Auth;
using PongServer.Application.Validators.Auth;

namespace PongServer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();

            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
