using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Application.Dtos.LayerResult;
using PongServer.Application.Dtos.V1.Games;

namespace PongServer.Application.Services.Games
{
    public interface IGameService
    {
        Task<GameDetailsDto> GetGameByIdAsync(Guid id);
        Task<ApplicationResult<GameDetailsDto>> CreateNewGameAsync(CreateGameDto dto);
        Task<ApplicationResult<bool>> UpdateScoreAsync(UpdateScoreDto scoreDto);
        Task EndGameAsync(Guid gameId);
    }
}
