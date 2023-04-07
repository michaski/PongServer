using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PongServer.Application.Dtos.V1.Hosts;
using PongServer.Domain.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace PongServer.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class HostsController : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(Summary = "List available hosts.")]
        public async Task<IActionResult> GetAll([FromQuery] QueryFilters filters)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{hostId}")]
        [SwaggerOperation(Summary = "Get host details")]
        public async Task<IActionResult> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Host a game.")]
        public async Task<IActionResult> CreateHost(CreateHostDto createHostDto)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{hostId}")]
        [SwaggerOperation(Summary = "Join game.")]
        public async Task<IActionResult> JoinGame(Guid hostId)
        {
            throw new NotSupportedException();
        }

        [HttpDelete("{hostId}")]
        [SwaggerOperation(Summary = "Shutdown game hosting.")]
        public async Task<IActionResult> DeleteHost(Guid hostId)
        {
            throw new NotImplementedException();
        }
    }
}
