using Microsoft.AspNetCore.Identity;
using PongServer.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace PongServer.Application.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersService(IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<CreatedUserDto> GetUserByIdAsync(Guid id)
        {
            return _mapper.Map<CreatedUserDto>(
                await _userManager.FindByIdAsync(id.ToString()));
        }
    }
}
