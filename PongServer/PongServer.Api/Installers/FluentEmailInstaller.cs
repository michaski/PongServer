using System.Net.Mail;

namespace PongServer.Api.Installers
{
    public class FluentEmailInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            var emailConfig = Configuration.GetSection("EmailConfig");
            var smtpConfig = emailConfig.GetSection("SmtpSender");
            services
                .AddFluentEmail(
                    emailConfig["Email"],
                    emailConfig["Name"])
                .AddSmtpSender(
                    smtpConfig["Host"],
                    int.Parse(smtpConfig["Port"]),
                    smtpConfig["Username"],
                    smtpConfig["Password"]);
        }
    }
}
