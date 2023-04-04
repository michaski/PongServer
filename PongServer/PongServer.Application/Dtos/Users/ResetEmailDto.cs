using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServer.Application.Dtos.Users
{
    public class ResetEmailDto
    {
        public string ChangeEmailToken { get; set; }
        public string NewEmail { get; set; }
    }
}
