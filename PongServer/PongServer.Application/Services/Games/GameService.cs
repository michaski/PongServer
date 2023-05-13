﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PongServer.Application.Dtos.LayerResult;
using PongServer.Application.Dtos.V1.Games;
using PongServer.Domain.Entities;
using PongServer.Domain.Interfaces;

namespace PongServer.Application.Services.Games
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IHostRepository _hostRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public GameService(
            IGameRepository gameRepository, 
            IHostRepository hostRepository, 
            UserManager<IdentityUser> userManager, 
            IMapper mapper)
        {
            _gameRepository = gameRepository;
            _hostRepository = hostRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<GameDetailsDto> GetGameByIdAsync(Guid id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            return _mapper.Map<GameDetailsDto>(game);
        }

        public async Task<ApplicationResult<GameDetailsDto>> CreateNewGameAsync(CreateGameDto dto)
        {
            var hostPlayer = await _userManager.FindByIdAsync(dto.HostPlayerId.ToString());
            var guestPlayer = await _userManager.FindByIdAsync(dto.GuestPlayerId.ToString());
            var host = await _hostRepository.GetHostByIdAsync(dto.HostId);

            if (hostPlayer is null)
            {
                return new ApplicationResult<GameDetailsDto>
                {
                    Status = 400,
                    Message = "Host player was not found."
                };
            }

            if (host is null)
            {
                return new ApplicationResult<GameDetailsDto>
                {
                    Status = 400,
                    Message = "Host with this id was not found."
                };
            }

            if (host.Owner.Id != hostPlayer.Id)
            {
                return new ApplicationResult<GameDetailsDto>
                {
                    Status = 400,
                    Message = "This player doesn't own this host."
                };
            }

            if (guestPlayer is null)
            {
                return new ApplicationResult<GameDetailsDto>
                {
                    Status = 400,
                    Message = "Guest player was not found."
                };
            }

            var newGame = new Game()
            {
                Host = host,
                HostId = dto.HostId,
                HostPlayer = hostPlayer,
                GuestPlayer = guestPlayer,
                GameStartTime = DateTime.UtcNow,
                LastUpdateTime = DateTime.UtcNow
            };
            var createdGame = await _gameRepository.CreateGameAsync(newGame);
            var mappedGame = _mapper.Map<Game, GameDetailsDto>(createdGame);
            return new ApplicationResult<GameDetailsDto>
            {
                Status = 203,
                Result = mappedGame
            };
        }
    }
}