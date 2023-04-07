using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PongServer.Application.Dtos.V1.Hosts;
using PongServer.Application.Services.Hosts;
using PongServer.Domain.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace PongServer.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class HostsController : ControllerBase
    {
        private readonly IHostService _hostService;

        public HostsController(IHostService hostService)
        {
            _hostService = hostService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "List available hosts.")]
        public async Task<IActionResult> GetAll([FromQuery] QueryFilters filters)
        {
            var result = await _hostService.GetAvailableHostsAsync(filters);
            if (!result.Items.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{hostId}")]
        [SwaggerOperation(Summary = "Get host details")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var hostInfo = await _hostService.GetHostByIdAsync(id);
            if (hostInfo is null)
            {
                return NotFound();
            }
            return Ok(hostInfo);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Host a game.")]
        public async Task<IActionResult> CreateHost(CreateHostDto createHostDto)
        {
            var newHost = await _hostService.CreateHostAsync(createHostDto);
            return CreatedAtAction("GetById", new { Id = newHost.Id }, newHost);
        }

        [HttpPut("{hostId}")]
        [SwaggerOperation(Summary = "Join game.")]
        public async Task<IActionResult> JoinGame(Guid hostId)
        {
            await _hostService.JoinGameAsync(hostId);
            return NoContent();
        }

        [HttpDelete("{hostId}")]
        [SwaggerOperation(Summary = "Shutdown game hosting.")]
        public async Task<IActionResult> DeleteHost(Guid hostId)
        {
            await _hostService.DeleteHostAsync(hostId);
            return NoContent();
        }
    }
}
