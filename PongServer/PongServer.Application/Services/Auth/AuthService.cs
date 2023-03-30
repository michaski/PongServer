using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PongServer.Application.Dtos.Auth;
using PongServer.Domain.Exceptions.Auth;

namespace PongServer.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public AuthService(UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<CreatedUserDto> RegisterNewUserAsync(RegisterUserDto userDto)
        {
            var registrationResult = await _userManager.CreateAsync(_mapper.Map<IdentityUser>(userDto));
            if (!registrationResult.Succeeded)
            {
                throw new IdentityException("Failed to register the user.",  registrationResult.Errors);
            }
            return _mapper.Map<CreatedUserDto>(
                await _userManager.FindByEmailAsync(userDto.Email));
        }

        public async Task<CreatedUserDto> GetUserByIdAsync(Guid id)
        {
            return _mapper.Map<CreatedUserDto>(
                await _userManager.FindByIdAsync(id.ToString()));
        }
    }
}
