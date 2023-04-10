using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PongServer.Application.Dtos.V1.PlayerScores;
using PongServer.Application.Services.PlayerScores;
using PongServer.Domain.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace PongServer.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ScoresController : ControllerBase
    {
        private readonly IPlayerScoreService _playerScoreService;

        public ScoresController(IPlayerScoreService playerScoreService)
        {
            _playerScoreService = playerScoreService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get player ranking list.")]
        public async Task<IActionResult> GetPlayerRanking(QueryFilters filters)
        {
            var result = await _playerScoreService.GetPlayerRankingAsync(filters);
            if (!result.Items.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update player's score after a match.")]
        public async Task<IActionResult> UpdatePlayerScore(PlayerScoreDto scoreDto)
        {
            var succeeded = await _playerScoreService.UpdatePlayerScoreAsync(scoreDto);
            if (!succeeded)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
