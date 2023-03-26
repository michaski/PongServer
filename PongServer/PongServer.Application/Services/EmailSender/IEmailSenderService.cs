using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Domain.Enums;

namespace PongServer.Application.Services.EmailSender
{
    public interface IEmailSenderService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
        Task<bool> SendEmailAsync(string to, string subject, EmailTemplate template, object model);
    }
}
