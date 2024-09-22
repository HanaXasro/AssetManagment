using Infrastructure.Helper;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Domain.Service;


namespace Infrastructure.Service
{
    public class EmailService : IEmailService
    {
        private AppSettings appSetting;

        public EmailService(IConfiguration configuration)
        {
            appSetting = new();
            appSetting.RefreshTokenTTL = int.Parse(configuration.GetSection("AppSettings:RefreshTokenTTL").Value!);
            appSetting.Secret = configuration.GetSection("AppSettings:Secret").Value!;
            appSetting.EmailFrom = configuration.GetSection("AppSettings:EmailFrom").Value!;
            appSetting.SmtpHost = configuration.GetSection("AppSettings:SmtpHost").Value!;
            appSetting.SmtpPort = int.Parse(configuration.GetSection("AppSettings:SmtpPort").Value!);
            appSetting.SmtpUser = configuration.GetSection("AppSettings:SmtpUser").Value!;
            appSetting.SmtpPass = configuration.GetSection("AppSettings:SmtpPass").Value!;
        }
        public void Send(string to, string subject, string html, string? from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? appSetting.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(appSetting.SmtpHost, appSetting.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(appSetting.SmtpUser, appSetting.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
