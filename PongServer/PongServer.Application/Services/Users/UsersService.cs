using Microsoft.AspNetCore.Identity;
using PongServer.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PongServer.Application.Dtos.Users;
using PongServer.Application.Services.UserContext;
using PongServer.Domain.Entities;

namespace PongServer.Application.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserContextService _userContextService;

        public UsersService(
            IMapper mapper, 
            UserManager<IdentityUser> userManager,
            IUserContextService userContextService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _userContextService = userContextService;
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
                    Message = "Could not verify user."
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
                    Message = "User with given id was not found."
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
                    Message = "User with given id was not found."
                };
            }

            if (!await _userManager.CheckPasswordAsync(user, resetPasswordDto.OldPassword))
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Message = "Old password is incorrect."
                };
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordDto.ResetPasswordToken));

            var result = await _userManager.ResetPasswordAsync(
                user, 
                decodedToken, 
                resetPasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return new AccountAlterResult
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(error => error.Description)
                };
            }

            return new AccountAlterResult
            {
                Succeeded = true,
                Message = "Password changed."
            };
        }
    }
}
