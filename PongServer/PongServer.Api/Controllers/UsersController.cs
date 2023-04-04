using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PongServer.Application.Dtos.Users;
using PongServer.Application.Services.Users;
using Swashbuckle.AspNetCore.Annotations;

namespace PongServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets user by id")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _usersService.GetUserByIdAsync(id);
            if (user is null)
            {
                return NotFound("User with given id was not found.");
            }
            return Ok(user);
        }

        [HttpPut("changeNick")]
        [SwaggerOperation(Summary = "Change user's nick.")]
        public async Task<IActionResult> ChangeNick(ChangeNickDto changeNickDto)
        {
            var result = await _usersService.ChangeNickAsync(changeNickDto);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }
            return NoContent();
        }

        [HttpPut("changePassword")]
        [SwaggerOperation(Summary = "Change user's password. Required email verification.")]
        public async Task<IActionResult> ChangePassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await _usersService.ChangePasswordAsync(resetPasswordDto);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }
            return NoContent();
        }

        [HttpDelete()]
        [SwaggerOperation(Summary = "Delete account.")]
        public async Task<IActionResult> Delete()
        {
            var result = await _usersService.DeleteUserAsync();
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return NoContent();
        }
    }
}
