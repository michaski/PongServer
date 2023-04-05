using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PongServer.Application.Dtos.V1.Auth;

namespace PongServer.Application.Validators.Auth
{
    internal class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(dto => dto.Nick)
                .NotEmpty()
                .NotNull()
                .MinimumLength(4)
                .MaximumLength(25);
            RuleFor(dto => dto.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();
            RuleFor(dto => dto.Password)
                .NotEmpty()
                .NotNull()
                .MinimumLength(6)
                .Must((dto, password) => dto.Password.Equals(dto.ConfirmPassword))
                .WithMessage("Passwords do not match.")
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")
                .WithMessage("Password must contain at least one lowercase and one uppercase character, one digit and one symbol.");
        }
    }
}
