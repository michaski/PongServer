using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServer.Domain.Entities
{
    public class AuthenticationResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public object Payload { get; set; }
    }
}
