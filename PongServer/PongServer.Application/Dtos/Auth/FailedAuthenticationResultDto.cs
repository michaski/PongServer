using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PongServer.Application.Mappings;
using PongServer.Domain.Entities;

namespace PongServer.Application.Dtos.Auth
{
    public class FailedAuthenticationResultDto : IMap
    {
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AuthenticationResult, FailedAuthenticationResultDto>();
            profile.CreateMap<AccountAlterResult, FailedAuthenticationResultDto>();
        }
    }
}
