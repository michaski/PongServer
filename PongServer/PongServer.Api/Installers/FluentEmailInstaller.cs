using System.Net;
using System.Net.Mail;
using PongServer.Application.Services.EmailSender;

namespace PongServer.Api.Installers
{
    public class FluentEmailInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            var emailConfig = Configuration.GetSection("EmailConfig");
            var smtpConfig = emailConfig.GetSection("SmtpSender");

            var smtpClient = new SmtpClient
            {
                Host = smtpConfig["Host"],
                Port = int.Parse(smtpConfig["Port"]),
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(smtpConfig["Username"], smtpConfig["Password"])
            };

            services
                .AddFluentEmail(
                    emailConfig["Email"],
                    emailConfig["Name"])
                .AddRazorRenderer()
                .AddSmtpSender(smtpClient);

            services.AddScoped<IEmailSenderService, EmailSenderService>();
        }
    }
}
