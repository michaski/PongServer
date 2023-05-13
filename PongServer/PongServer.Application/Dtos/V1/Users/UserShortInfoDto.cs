using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PongServer.Application.Mappings;

namespace PongServer.Application.Dtos.V1.Users
{
    public class UserShortInfoDto : IMap
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IdentityUser, UserShortInfoDto>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(user => user.UserName));
        }
    }
}
