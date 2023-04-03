using Microsoft.AspNetCore.Identity;
using PongServer.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Domain.Entities;

namespace PongServer.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthenticationResult> RegisterNewUserAsync(RegisterUserDto userDto);
        Task<bool> SendAccountActivationLinkAsync(string email);
        Task<AuthenticationResult> ConfirmEmailAsync(string userId, string activationCode);
        Task<AuthenticationResult> AuthenticateUserAsync(LoginUserDto loginDto);
    }
}
