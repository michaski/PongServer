using PongServer.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServer.Application.Services.Users
{
    public interface IUsersService
    {
        Task<CreatedUserDto> GetUserByIdAsync(Guid id);
    }
}
