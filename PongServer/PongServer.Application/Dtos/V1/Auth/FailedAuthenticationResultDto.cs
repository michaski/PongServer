using AutoMapper;
using PongServer.Application.Mappings;
using PongServer.Domain.Entities;

namespace PongServer.Application.Dtos.V1.Auth
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
