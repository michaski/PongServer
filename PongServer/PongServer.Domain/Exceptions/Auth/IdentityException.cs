using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PongServer.Domain.Exceptions.Auth
{
    public class IdentityException : Exception
    {
        public string Message { get; set; }
        public List<IdentityError> Errors { get; set; }

        public IdentityException(string message) 
            : base(message) { }

        public IdentityException(string message, IEnumerable<IdentityError> errors)
            : base($"{message}\nErrors:\n{string.Join(Environment.NewLine, errors)}")
        {
            Errors = errors.ToList();
            Message = message;
        }
    }
}
