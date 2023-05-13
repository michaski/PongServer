using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PongServer.Application.Dtos.V1.Games;
using PongServer.Application.Services.Games;
using Swashbuckle.AspNetCore.Annotations;

namespace PongServer.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets game by id.")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new game for host.")]
        public async Task<IActionResult> CreateNewGame(CreateGameDto dto)
        {
            var result = await _gameService.CreateNewGameAsync(dto);
            if (result.Status == 400)
            {
                return BadRequest(result.Message);
            }
            var createdGame = result.Result;
            return CreatedAtAction("GetById", new { id = createdGame.Id }, createdGame);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "End game with given id.")]
        public async Task<IActionResult> EndGame(Guid id)
        {
            await _gameService.EndGameAsync(id);
            return NoContent();
        }
    }
}
