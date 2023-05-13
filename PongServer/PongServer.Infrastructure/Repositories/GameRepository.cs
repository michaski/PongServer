using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PongServer.Domain.Entities;
using PongServer.Domain.Interfaces;
using PongServer.Infrastructure.Data;

namespace PongServer.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly PongDataContext _context;

        public GameRepository(PongDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetStaleGamesAsync(TimeSpan minStaleTime)
        {
            return await _context.Games
                .Where(game => DateTime.UtcNow - game.LastUpdateTime >= minStaleTime)
                .ToListAsync();
        }

        public async Task<Game> GetByIdAsync(Guid id)
        {
            return await _context.Games
                .Include(game => game.HostPlayer)
                .Include(game => game.GuestPlayer)
                .Include(game => game.Host)
                .FirstOrDefaultAsync(game => game.Id == id);
        }

        public async Task<Game> CreateGameAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task UpdateGameAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGameAsync(Game game)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }
    }
}
