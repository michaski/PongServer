using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PongServer.Application.Dtos.V1.Hosts;

namespace PongServer.Application.Validators.Hosts
{
    public class CreateHostValidator : AbstractValidator<CreateHostDto>
    {
        public CreateHostValidator()
        {
            RuleFor(dto => dto.Ip)
                .NotEmpty()
                .MinimumLength(7)
                .MaximumLength(15)
                .Matches("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")
                .WithMessage("Specified string is not a valid IPv4 address.");
            RuleFor(dto => dto.Name)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(32);
            RuleFor(dto => dto.Port)
                .GreaterThan(0)
                .LessThan(65535);
        }
    }
}
