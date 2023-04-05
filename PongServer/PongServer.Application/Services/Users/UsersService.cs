using Microsoft.AspNetCore.Identity;
using PongServer.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PongServer.Application.Dtos.Users;
using PongServer.Application.Services.EmailSender;
using PongServer.Application.Services.UserContext;
using PongServer.Domain.Entities;
using PongServer.Domain.Enums;

namespace PongServer.Application.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserContextService _userContextService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersService(
            IMapper mapper, 
            UserManager<IdentityUser> userManager,
            IUserContextService userContextService,
            IEmailSenderService emailSenderService,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _userManager = userManager;
            _userContextService = userContextService;
            _emailSenderService = emailSenderService;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreatedUserDto> GetUserByIdAsync(Guid id)
        {
            return _mapper.Map<CreatedUserDto>(
                await _userManager.FindByIdAsync(id.ToString()));
        }

        public async Task<AccountAlterResult> ChangeNickAsync(ChangeNickDto changeNickDto)
        {
            var user = await _userManager.FindByIdAsync(_userContextService.UserId);
            if (user is null)
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Failed to change user name.",
                    Errors = new []
                    {
                        "Could not verify user."
                    }
                };
            }

            var result = await _userManager.SetUserNameAsync(user, changeNickDto.Nick);
            if (!result.Succeeded)
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Failed to change user name.",
                    Errors = result.Errors.Select(err => err.Description)
                };
            }

            return new AccountAlterResult
            {
                Succeeded = true,
                Message = "Username changed successfully."
            };
        }

        public async Task<AccountAlterResult> DeleteUserAsync()
        {
            var user = await _userManager.FindByIdAsync(_userContextService.UserId);
            if (user is null)
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Could not delete user.",
                    Errors = new[]
                    {
                        "Could not verify user."
                    }
                };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Could not delete user.",
                    Errors = result.Errors.Select(err => err.Description)
                };
            }

            return new AccountAlterResult
            {
                Succeeded = true,
                Message = "Account deleted."
            };
        }

        public async Task<AccountAlterResult> ChangePasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByIdAsync(_userContextService.UserId);
            if (user is null)
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Password change failed.",
                    Errors = new[]
                    {
                        "Could not verify user."
                    }
                };
            }

            if (!await _userManager.CheckPasswordAsync(user, resetPasswordDto.OldPassword))
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Password change failed.",
                    Errors = new[]
                    {
                        "Old password is incorrect."
                    }
                };
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordDto.ResetPasswordToken));
            var isValidToken = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider,
                UserManager<IdentityUser>.ResetPasswordTokenPurpose, decodedToken);
            if (!isValidToken)
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Password change failed.",
                    Errors = new[]
                    {
                        "Reset password token is not valid."
                    }
                };
            }

            var result = await _userManager.ChangePasswordAsync(
                user, 
                resetPasswordDto.OldPassword, 
                resetPasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Password change failed.",
                    Errors = result.Errors.Select(error => error.Description)
                };
            }

            return new AccountAlterResult
            {
                Succeeded = true,
                Message = "Password changed."
            };
        }

        public async Task<AccountAlterResult> ChangeEmailAsync(ResetEmailDto resetEmailDto)
        {
            var user = await _userManager.FindByIdAsync(_userContextService.UserId);
            if (user == null)
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Email change failed.",
                    Errors = new[]
                    {
                        "Could not verify user."
                    }
                };
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetEmailDto.ChangeEmailToken));
            var result = await _userManager.ChangeEmailAsync(user, resetEmailDto.NewEmail, decodedToken);
            if (!result.Succeeded)
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Email change failed.",
                    Errors = result.Errors.Select(error => error.Description)
                };
            }

            var activationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            activationCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(activationCode));

            var emailSent = await _emailSenderService.SendEmailAsync(
                resetEmailDto.NewEmail, 
                "New Email Verification", 
                EmailTemplate.EmailVerification, 
                new
                {
                    Nick = user.UserName,
                    ActivationLink = _linkGenerator.GetUriByAction(
                        _httpContextAccessor.HttpContext,
                        action: "ConfirmEmail",
                        controller: "Auth",
                        values: new
                        {
                            userId = user.Id,
                            activationCode = activationCode
                        })
                });

            if (!emailSent)
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Email change failed.",
                    Errors = new[]
                    {
                        "Could not send verification email."
                    }
                };
            }

            return new AccountAlterResult
            {
                Succeeded = true,
                Message = "Email changed. Please activate your new email."
            };
        }
    }
}
