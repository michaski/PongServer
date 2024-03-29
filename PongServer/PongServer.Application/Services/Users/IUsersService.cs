﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Application.Dtos.V1.Auth;
using PongServer.Application.Dtos.V1.Users;
using PongServer.Domain.Entities;

namespace PongServer.Application.Services.Users
{
    public interface IUsersService
    {
        Task<CreatedUserDto> GetUserByIdAsync(Guid id);
        Task<AccountAlterResult> ChangeNickAsync(ChangeNickDto changeNickDto);
        Task<AccountAlterResult> DeleteUserAsync();
        Task<AccountAlterResult> ChangePasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<AccountAlterResult> ChangeEmailAsync(ResetEmailDto resetEmailDto);
    }
}
