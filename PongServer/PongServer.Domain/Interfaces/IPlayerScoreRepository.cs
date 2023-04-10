using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PongServer.Domain.Entities;
using PongServer.Domain.Utils;

namespace PongServer.Domain.Interfaces
{
    public interface IPlayerScoreRepository
    {
        Task<ResultPage<PlayerScore>> GetScoreListAsync(QueryFilters filters);
        Task<PlayerScore> GetPlayersScoreAsync(IdentityUser player);
        Task<PlayerScore> CreateScoreForPlayerAsync(IdentityUser player);
        Task UpdatePlayerScoreAsync(PlayerScore updatedScore);
        Task DeletePlayersScoreAsync(PlayerScore playerScore);
    }
}
