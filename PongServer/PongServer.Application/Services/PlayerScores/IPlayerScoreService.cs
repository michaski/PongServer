using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Application.Dtos.V1.Pagination;
using PongServer.Application.Dtos.V1.PlayerScores;
using PongServer.Domain.Utils;

namespace PongServer.Application.Services.PlayerScores
{
    public interface IPlayerScoreService
    {
        Task<PagedResult<PlayerRankingListDto>> GetPlayerRankingAsync(QueryFilters filters);
        Task<bool> UpdatePlayerScoreAsync(PlayerScoreDto scoreDto);
    }
}
