using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PongServer.Application.Dtos.Auth;
using PongServer.Application.Services.Auth;
using PongServer.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace PongServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("/register")]
        [SwaggerOperation(Summary = "Creates new user account with need for activation.")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto userDto)
        {
            var result = await _authService.RegisterNewUserAsync(userDto);
            if (!result.Succeeded)
            {
                return BadRequest(_mapper.Map<AuthenticationResult, FailedAuthenticationResultDto>(result));
            }

            var createdUserDto = result.Payload as CreatedUserDto;
            return CreatedAtAction("GetUserById", "Users", new { id = createdUserDto.Id }, createdUserDto);
        }

        [HttpGet("confirmEmail", Name = "ConfirmEmail")]
        [SwaggerOperation(Summary = "Confirms email.")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string activationCode)
        {
            var result = await _authService.ConfirmEmailAsync(userId, activationCode);
            if (result.Succeeded)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(_mapper.Map<AuthenticationResult, FailedAuthenticationResultDto>(result));
            }
        }

        [HttpPost("/login")]
        [SwaggerOperation(Summary = "Authenticates user and returns JWT token.")]
        public async Task<IActionResult> Login(LoginUserDto loginDto)
        {
            var loginResult = await _authService.AuthenticateUserAsync(loginDto);
            if (!loginResult.Succeeded)
            {
                return Unauthorized(_mapper.Map<AuthenticationResult, FailedAuthenticationResultDto>(loginResult));  
            }
            return Ok(new
            {
                Token = loginResult.Token
            });
        }

        [Authorize]
        [HttpPut("requestPasswordChange")]
        [SwaggerOperation(Summary = "Sends password change token to user's email.")]
        public async Task<IActionResult> RequestPasswordChange()
        {
            var result = await _authService.SendPasswordResetTokenAsync();
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [Authorize]
        [HttpPut("requestEmailChange")]
        [SwaggerOperation(Summary = "Sends email change token to user's email.")]
        public async Task<IActionResult> RequestEmailChange(ChangeEmailDto changeEmailDto)
        {
            var result = await _authService.SendEmailResetTokenAsync(changeEmailDto);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
