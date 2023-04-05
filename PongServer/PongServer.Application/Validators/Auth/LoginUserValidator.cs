using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PongServer.Application.Dtos.V1.Auth;

namespace PongServer.Application.Validators.Auth
{
    internal class LoginUserValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(dto => dto.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();
            RuleFor(dto => dto.Password)
                .NotEmpty();
        }
    }
}
