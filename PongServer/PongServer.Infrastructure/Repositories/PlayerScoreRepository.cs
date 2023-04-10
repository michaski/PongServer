using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            return await _context.Scores
                .FirstOrDefaultAsync(score => score.Player.Id == player.Id);
        }

        public async Task<PlayerScore> CreateScoreForPlayerAsync(IdentityUser player)
        {
            var playerScore = new PlayerScore();
            playerScore.Player = player;
            _context.Scores.Add(playerScore);
            await _context.SaveChangesAsync();
            return playerScore;
        }

        public async Task UpdateScoreAfterGameAsync(PlayerScore firstScore, PlayerScore secondScore)
        {
            _context.Scores.Update(firstScore);
            _context.Scores.Update(secondScore);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlayersScoreAsync(PlayerScore playerScore)
        {
            _context.Scores.Remove(playerScore);
            await _context.SaveChangesAsync();
        }
    }
}
