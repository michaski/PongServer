using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using PongServer.Domain.Enums;
using PongServer.Domain.HelperMethods;

namespace PongServer.Application.Services.EmailSender
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IFluentEmail _email;
        private readonly ILogger<EmailSenderService> _logger;
        private const string EmailTemplatePath = "PongServer.Application.Services.EmailSender.EmailTemplates.{0}.cshtml";

        public EmailSenderService(IFluentEmail email, ILogger<EmailSenderService> logger)
        {
            _email = email;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            var result = await _email
                .To(to)
                .Subject(subject)
                .Body(body)
                .SendAsync();
            if (!result.Successful)
            {
                _logger.LogError($"Failed to send email to {to}, subject: {subject}. Errors:\n{String.Join(Environment.NewLine, result.ErrorMessages)}");
            }
            else
            {
                _logger.LogDebug($"Successfully sent email to {to}, subject: {subject}.");
            }
            return result.Successful;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, EmailTemplate template, object model)
        {
            var templatePath = string.Format(EmailTemplatePath, template);
            var result = await _email
                .To(to)
                .Subject(subject)
                .UsingTemplateFromEmbedded(templatePath, ExpandoObjectHelper.ToExpandoObject(model), GetType().Assembly)
                .SendAsync();
            if (!result.Successful)
            {
                _logger.LogError($"Failed to send email to {to}, subject: {subject}. Errors:\n{String.Join(Environment.NewLine, result.ErrorMessages)}");
            }
            else
            {
                _logger.LogDebug($"Successfully sent email to {to}, subject: {subject}.");
            }
            return result.Successful;
        }
    }
}
