using AutoMapper;
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

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets user by id")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user is null)
            {
                return NotFound("User with given id was not found.");
            }
            return Ok(user);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates new user account with need for activation.")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto userDto)
        {
            var result = await _authService.RegisterNewUserAsync(userDto);
            if (!result.Succeeded)
            {
                return BadRequest(_mapper.Map<AuthenticationResult, FailedAuthenticationResultDto>(result));
            }

            var createdUserDto = result.Payload as CreatedUserDto;
            return CreatedAtAction("GetUserById", new { id = createdUserDto.Id }, createdUserDto);
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
    }
}
