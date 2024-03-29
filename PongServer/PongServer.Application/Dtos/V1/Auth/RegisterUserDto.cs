﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PongServer.Application.Mappings;

namespace PongServer.Application.Dtos.V1.Auth
{
    public class RegisterUserDto : IMap
    {
        public string Email { get; set; }
        public string Nick { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterUserDto, IdentityUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Nick));
        }
    }
}
