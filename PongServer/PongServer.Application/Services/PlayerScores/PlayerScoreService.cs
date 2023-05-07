using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PongServer.Application.Dtos.V1.Pagination;
using PongServer.Application.Dtos.V1.PlayerScores;
using PongServer.Application.Services.UserContext;
using PongServer.Domain.Enums;
using PongServer.Domain.HelperMethods;
using PongServer.Domain.Interfaces;
using PongServer.Domain.Utils;

namespace PongServer.Application.Services.PlayerScores
{
    public class PlayerScoreService : IPlayerScoreService
    {
        private readonly IPlayerScoreRepository _playerScoreRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public PlayerScoreService(
            IPlayerScoreRepository playerScoreRepository,
            UserManager<IdentityUser> userManager,
            IUserContextService userContextService,
            IMapper mapper)
        {
            _playerScoreRepository = playerScoreRepository;
            _userManager = userManager;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<PagedResult<PlayerRankingListDto>> GetPlayerRankingAsync(QueryFilters filters)
        {
            var result = await _playerScoreRepository.GetScoreListAsync(filters);
            var mappedItems = _mapper.Map<IEnumerable<PlayerRankingListDto>>(result.Items).ToList();
            var resultPage = new ResultPage<PlayerRankingListDto>(mappedItems, result.TotalItemsCount);
            return new PagedResult<PlayerRankingListDto>(resultPage, filters);
        }

        public async Task<bool> UpdatePlayerScoreAsync(PlayerScoreDto scoreDto)
        {
            var player = await _userManager.FindByIdAsync(_userContextService.UserId);
            if (player == null)
            {
                return false;
            }

            var opponent = await _userManager.FindByIdAsync(scoreDto.OpponentId.ToString());
            if (opponent == null)
            {
                return false;
            }

            var playerScore = await _playerScoreRepository.GetPlayersScoreAsync(player);
            var opponentScore = await _playerScoreRepository.GetPlayersScoreAsync(opponent);

            playerScore.GamesPlayed += 1;
            playerScore.GamesWon += (int)scoreDto.MatchResult;
            playerScore.RankingScore = EloScoreCalculator.CalculateScore(scoreDto.MatchResult, 
                playerScore.RankingScore, opponentScore.RankingScore);

            var oppositeMatchResult = scoreDto.MatchResult == GameResult.Won ? GameResult.Lost : GameResult.Won;
            opponentScore.GamesPlayed += 1;
            opponentScore.GamesWon += (int)oppositeMatchResult;
            opponentScore.RankingScore = EloScoreCalculator.CalculateScore(oppositeMatchResult,
                opponentScore.RankingScore, playerScore.RankingScore);

            await _playerScoreRepository.UpdateScoreAfterGameAsync(playerScore, opponentScore);

            playerScore.RankingPosition = await _playerScoreRepository.GetPlayerPositionAsync(playerScore);
            opponentScore.RankingPosition = await _playerScoreRepository.GetPlayerPositionAsync(opponentScore);
            await _playerScoreRepository.UpdateScoreAfterGameAsync(playerScore, opponentScore);

            return true;
        }
    }
}
