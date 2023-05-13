using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Domain.Entities;

namespace PongServer.Domain.Interfaces
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetStaleGamesAsync(TimeSpan minStaleTime);
        Task<Game> GetByIdAsync(Guid id);
        Task<Game> CreateGameAsync(Game game);
        Task UpdateGameAsync(Game game);
        Task DeleteGameAsync(Game game);
    }
}
