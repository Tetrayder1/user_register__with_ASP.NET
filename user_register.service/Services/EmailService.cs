
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using user_register.core.OptionsModels;

namespace user_register.service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettingsOptions _emailSettings;

        public EmailService(IOptions<EmailSettingsOptions> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetEmailLink, string ToEmail)
        {
            var smptClinet = new SmtpClient();

            smptClinet.Host = _emailSettings.Host!;
            smptClinet.DeliveryMethod= SmtpDeliveryMethod.Network;
            smptClinet.UseDefaultCredentials = false;
            smptClinet.Port = 587;
            smptClinet.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smptClinet.EnableSsl = true;
            

            var MailMessage = new MailMessage();
            MailMessage.From = new MailAddress(_emailSettings.Email);
            MailMessage.To.Add(ToEmail);

            MailMessage.Subject = "Password change link";
            MailMessage.Body = @$"<h3>Sifreni yenilemek ucun asaqidaki linke click et</h3>
                                   <p><a href='{resetEmailLink}'>Password Change Link</a></p>";
            MailMessage.IsBodyHtml=true;    

            await smptClinet.SendMailAsync(MailMessage);
          
        }
    }
}
