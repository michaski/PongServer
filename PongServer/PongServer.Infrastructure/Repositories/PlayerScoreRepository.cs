using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PongServer.Domain.Entities;
using PongServer.Domain.Interfaces;
using PongServer.Domain.Utils;
using PongServer.Infrastructure.Data;

namespace PongServer.Infrastructure.Repositories
{
    public class PlayerScoreRepository : IPlayerScoreRepository
    {
        private readonly PongDataContext _context;

        public PlayerScoreRepository(PongDataContext context)
        {
            _context = context;
        }

        public Task<ResultPage<PlayerScore>> GetScoreListAsync(QueryFilters filters)
        {
            throw new NotImplementedException();
        }

        public async Task<PlayerScore> GetPlayersScoreAsync(IdentityUser player)
        {
            throw new NotImplementedException();
        }

        public async Task<PlayerScore> CreateScoreForPlayerAsync(IdentityUser player)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatePlayerScoreAsync(PlayerScore updatedScore)
        {
            throw new NotImplementedException();
        }

        public async Task DeletePlayersScoreAsync(PlayerScore player)
        {
            throw new NotImplementedException();
        }
    }
}
