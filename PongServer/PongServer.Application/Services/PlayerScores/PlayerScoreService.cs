using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PongServer.Application.Dtos.V1.Pagination;
using PongServer.Application.Dtos.V1.PlayerScores;
using PongServer.Application.Services.UserContext;
using PongServer.Domain.Interfaces;
using PongServer.Domain.Utils;

namespace PongServer.Application.Services.PlayerScores
{
    public class PlayerScoreService : IPlayerScoreService
    {
        private readonly IPlayerScoreRepository _playerScoreRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserContextService _userContextService;

        public PlayerScoreService(
            IPlayerScoreRepository playerScoreRepository, 
            UserManager<IdentityUser> userManager, 
            IUserContextService userContextService)
        {
            _playerScoreRepository = playerScoreRepository;
            _userManager = userManager;
            _userContextService = userContextService;
        }

        public async Task<PagedResult<PlayerRankingListDto>> GetPlayerRankingAsync(QueryFilters filters)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePlayerScoreAsync(PlayerScoreDto scoreDto)
        {
            throw new NotImplementedException();
        }
    }
}
