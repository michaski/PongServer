using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PongServer.Application.Mappings;
using PongServer.Domain.Entities;

namespace PongServer.Application.Dtos.V1.Games
{
    public class GameDetailsDto : IMap
    {
        public Guid Id { get; set; }
        public Host Host { get; set; }
        public IdentityUser HostPlayer { get; set; }
        public IdentityUser GuestPlayer { get; set; }
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
