using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PongServer.Application.Services.UserContext
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public string UserId =>
            User is null ? null : User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
    }
}
