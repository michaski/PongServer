using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using PongServer.Application.Dtos.Auth;
using PongServer.Application.Services.EmailSender;
using PongServer.Domain.Entities;
using PongServer.Domain.Enums;
using PongServer.Domain.Exceptions.Auth;

namespace PongServer.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public AuthService(
            UserManager<IdentityUser> userManager, 
            IMapper mapper, 
            IEmailSenderService emailSenderService, 
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailSenderService = emailSenderService;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
        }

        public async Task<AuthenticationResult> RegisterNewUserAsync(RegisterUserDto userDto)
        {
            var registrationResult = await _userManager.CreateAsync(_mapper.Map<IdentityUser>(userDto));
            if (!registrationResult.Succeeded)
            {
                return new AuthenticationResult()
                {
                    Succeeded = false,
                    Message = "Failed to register the user.",
                    Errors = registrationResult.Errors.Select(error => error.Description)
                };
            }

            var emailSent = await SendAccountActivationLinkAsync(userDto.Email);
            if (!emailSent)
            {
                return new AuthenticationResult()
                {
                    Succeeded = false,
                    Message = "Failed to send confirmation email. Please try again."
                };
            }

            var createdUser = await _userManager.FindByEmailAsync(userDto.Email);
            return new AuthenticationResult()
            {
                Succeeded = true,
                Message = "Account created successfully. Please check your email for verification link.",
                Payload = _mapper.Map<IdentityUser, CreatedUserDto>(createdUser)
            };
        }

        public async Task<CreatedUserDto> GetUserByIdAsync(Guid id)
        {
            return _mapper.Map<CreatedUserDto>(
                await _userManager.FindByIdAsync(id.ToString()));
        }

        public async Task<bool> SendAccountActivationLinkAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var activationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            activationCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(activationCode));
            var succeeded = await _emailSenderService.SendEmailAsync(
                user.Email, 
                "Email verification", 
                EmailTemplate.EmailVerification,
                new { ActivationLink = GetActivationLink(user, activationCode), Nick = user.UserName });
            return succeeded;
        }

        public async Task<AuthenticationResult> ConfirmEmailAsync(string userId, string activationCode)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new AuthenticationResult()
                {
                    Succeeded = false,
                    Message = "User with given id does not exist."
                };
            }
            var decodedActivationCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(activationCode));
            var result = await _userManager.ConfirmEmailAsync(user, decodedActivationCode);

            if (!result.Succeeded)
            {
                return new AuthenticationResult()
                {
                    Succeeded = false,
                    Message = "Failed to confirm the email.",
                    Errors = result.Errors.Select(error => error.Description)
                };
            }
            return new AuthenticationResult()
            {
                Succeeded = true,
                Message = "Email confirmed."
            };
        }

        private string GetActivationLink(IdentityUser user, string activationCode)
        {
            return _linkGenerator.GetUriByAction(
                _httpContextAccessor.HttpContext,
                action: "ConfirmEmail",
                values: new
                {
                    userId = user.Id,
                    activationCode = activationCode
                });
        }
    }
}
