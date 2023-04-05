using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PongServer.Application.Dtos.V1.Users;

namespace PongServer.Application.Validators.Users
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator()
        {
            RuleFor(dto => dto.ResetPasswordToken)
                .NotEmpty();
            RuleFor(dto => dto.OldPassword)
                .NotEmpty()
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")
                .WithMessage("Password must contain at least one lowercase and one uppercase character, one digit and one symbol.");
            RuleFor(dto => dto.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")
                .WithMessage("Password must contain at least one lowercase and one uppercase character, one digit and one symbol.");
            RuleFor(dto => dto.ConfirmNewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Must((dto, password) => dto.NewPassword == dto.ConfirmNewPassword)
                .WithMessage("Passwords do not match.")
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")
                .WithMessage("Password must contain at least one lowercase and one uppercase character, one digit and one symbol.");
        }
    }
}
