using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PongServer.Application.Services.UserContext
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        string UserId { get; }

    }
}
