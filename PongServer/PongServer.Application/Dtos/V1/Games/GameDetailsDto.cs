using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PongServer.Application.Dtos.V1.Hosts;
using PongServer.Application.Dtos.V1.Users;
using PongServer.Application.Mappings;
using PongServer.Domain.Entities;

namespace PongServer.Application.Dtos.V1.Games
{
    public class GameDetailsDto : IMap
    {
        public Guid Id { get; set; }
        public HostDetailsDto Host { get; set; }
        public UserShortInfoDto HostPlayer { get; set; }
        public UserShortInfoDto GuestPlayer { get; set; }
        public int HostPlayerScore { get; set; }
        public int GuestPlayerScore { get; set; }
        public DateTime GameStartTime { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Game, GameDetailsDto>();
        }
    }
}
