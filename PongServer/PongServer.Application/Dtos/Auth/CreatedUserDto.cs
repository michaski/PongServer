using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PongServer.Application.Mappings;

namespace PongServer.Application.Dtos.Auth
{
    public class CreatedUserDto : IMap
    {
        public Guid Id { get; set; }
        public string Nick { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IdentityUser, CreatedUserDto>()
                .ForMember(dest => dest.Nick, opt => opt.MapFrom(src => src.UserName));
        }
    }
}
