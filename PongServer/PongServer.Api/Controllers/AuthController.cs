using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PongServer.Application.Dtos.Auth;
using PongServer.Application.Services.Auth;
using Swashbuckle.AspNetCore.Annotations;

namespace PongServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
            var newUser = await _authService.RegisterNewUserAsync(userDto);
            return CreatedAtAction("GetUserById", new { id = newUser.Id }, newUser);
        }

        [HttpGet("confirmEmail", Name = "ConfirmEmail")]
        [SwaggerOperation(Summary = "Confirms email.")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string activationCode)
        {
            var succeeded = await _authService.ConfirmEmailAsync(userId, activationCode);
            if (succeeded)
            {
                return Ok("Email verified.");
            }
            else
            {
                return BadRequest("Email couldn't be verified.");
            }
        }
    }
}
