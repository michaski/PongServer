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

        public async Task<CreatedUserDto> RegisterNewUserAsync(RegisterUserDto userDto)
        {
            var registrationResult = await _userManager.CreateAsync(_mapper.Map<IdentityUser>(userDto));
            if (!registrationResult.Succeeded)
            {
                throw new IdentityException("Failed to register the user.",  registrationResult.Errors);
            }
            await SendAccountActivationLinkAsync(userDto.Email);
            return _mapper.Map<CreatedUserDto>(
                await _userManager.FindByEmailAsync(userDto.Email));
        }

        public async Task<CreatedUserDto> GetUserByIdAsync(Guid id)
        {
            return _mapper.Map<CreatedUserDto>(
                await _userManager.FindByIdAsync(id.ToString()));
        }

        public async Task SendAccountActivationLinkAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var activationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            activationCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(activationCode));
            var succeeded = await _emailSenderService.SendEmailAsync(
                user.Email, 
                "Email verification", 
                EmailTemplate.EmailVerification,
                new { ActivationLink = GetActivationLink(user, activationCode), Nick = user.UserName });
            if (!succeeded)
            {
                throw new Exception("Failed to send confirmation email. Please try again.");
            }
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string activationCode)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var decodedActivationCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(activationCode));
            var result = await _userManager.ConfirmEmailAsync(user, decodedActivationCode);
            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to confirm the email.", result.Errors);
            }
            return result.Succeeded;
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
