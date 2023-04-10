using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PongServer.Application.Mappings;
using PongServer.Domain.Entities;

namespace PongServer.Application.Dtos.V1.PlayerScores
{
    public class PlayerRankingListDto : IMap
    {
        public int Position { get; set; }
        public string Nick { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
        public int TotalGamesPlayed { get; set; }
        public int RankingScore { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PlayerScore, PlayerRankingListDto>()
                .ForMember(dto => dto.TotalGamesPlayed, cfg => cfg.MapFrom(score => score.GamesPlayed))
                .ForMember(dto => dto.GamesLost, cfg => cfg.MapFrom(score => score.GamesPlayed - score.GamesWon))
                .ForMember(dto => dto.Nick, cfg => cfg.MapFrom(score => score.Player.UserName));
        }
    }
}
