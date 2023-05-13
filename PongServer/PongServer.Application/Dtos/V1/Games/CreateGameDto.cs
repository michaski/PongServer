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
    public class CreateGameDto : IMap
    {
        public Guid HostId { get; set; }
        public Guid HostPlayerId { get; set; }
        public Guid GuestPlayerId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateGameDto, Game>();
        }
    }
}
