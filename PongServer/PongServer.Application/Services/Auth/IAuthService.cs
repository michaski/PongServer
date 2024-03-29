﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Application.Dtos.V1.Auth;
using PongServer.Domain.Entities;

namespace PongServer.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthenticationResult> RegisterNewUserAsync(RegisterUserDto userDto);
        Task<bool> SendAccountActivationLinkAsync(string email);
        Task<AuthenticationResult> ConfirmEmailAsync(string userId, string activationCode);
        Task<AuthenticationResult> AuthenticateUserAsync(LoginUserDto loginDto);
        Task<bool> SendPasswordResetTokenAsync();
        Task<bool> SendEmailResetTokenAsync(ChangeEmailDto changeEmailDto);
    }
}
