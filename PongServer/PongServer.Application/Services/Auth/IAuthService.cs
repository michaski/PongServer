using Microsoft.AspNetCore.Identity;
using PongServer.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServer.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<CreatedUserDto> RegisterNewUserAsync(RegisterUserDto userDto);

        Task<CreatedUserDto> GetUserByIdAsync(Guid id);
    }
}
