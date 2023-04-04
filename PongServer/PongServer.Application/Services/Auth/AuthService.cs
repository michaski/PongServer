using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PongServer.Application.Dtos.Auth;
using PongServer.Application.Services.EmailSender;
using PongServer.Application.Services.UserContext;
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
        private readonly IConfiguration _configuration;
        private readonly IUserContextService _userContextService;

        public AuthService(
            UserManager<IdentityUser> userManager, 
            IMapper mapper, 
            IEmailSenderService emailSenderService, 
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator,
            IConfiguration configuration,
            IUserContextService userContextService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailSenderService = emailSenderService;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _configuration = configuration;
            _userContextService = userContextService;
        }

        public async Task<AuthenticationResult> RegisterNewUserAsync(RegisterUserDto userDto)
        {
            var registrationResult = await _userManager.CreateAsync(_mapper.Map<IdentityUser>(userDto), userDto.Password);
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

        public async Task<AuthenticationResult> AuthenticateUserAsync(LoginUserDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                return new AuthenticationResult()
                {
                    Succeeded = false,
                    Message = "Login failed - wrong email or password."
                };
            }

            if (!user.EmailConfirmed)
            {
                return new AuthenticationResult()
                {
                    Succeeded = false,
                    Message = "Account has not been activated. Please check your email for activation link."
                };
            }

            var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordIsCorrect)
            {
                return new AuthenticationResult()
                {
                    Succeeded = false,
                    Message = "Login failed - wrong email or password."
                };
            }

            return new AuthenticationResult
            {
                Succeeded = true,
                Token = GetJwtToken(user.Id)
            };
        }

        public async Task<bool> SendPasswordResetTokenAsync()
        {
            var user = await _userManager.FindByIdAsync(_userContextService.UserId);
            if (user is null)
            {
                return false;
            }

            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordResetToken));
            var succeeded = await _emailSenderService.SendEmailAsync(
                user.Email, 
                "Password reset", 
                EmailTemplate.PasswordReset, 
                new
                {
                    Nick = user.UserName,
                    PasswordResetToken = encodedToken
                });

            return succeeded;
        }

        private string GetJwtToken(string userId)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] secret = Encoding.UTF8.GetBytes(_configuration["Authentication:JwtSecretKey"]);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
                Expires = DateTime.Now.AddHours(double.Parse(_configuration["Authentication:JwtExpireHours"])),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Authentication:JwtIssuer"],
                Audience = _configuration["Authentication:JwtIssuer"],
            };
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        private string GetActivationLink(IdentityUser user, string activationCode)
        {
            return _linkGenerator.GetUriByAction(
                _httpContextAccessor.HttpContext,
                action: "ConfirmEmail",
                controller: "Auth",
                values: new
                {
                    userId = user.Id,
                    activationCode = activationCode
                });
        }
    }
}
