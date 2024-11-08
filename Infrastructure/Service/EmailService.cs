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
        private AppSettings _appSetting;

        public EmailService(IConfiguration configuration)
        {
            _appSetting = new();
            _appSetting.RefreshTokenTTL = int.Parse(configuration.GetSection("AppSettings:RefreshTokenTTL").Value!);
            _appSetting.Secret = configuration.GetSection("AppSettings:Secret").Value!;
            _appSetting.EmailFrom = configuration.GetSection("AppSettings:EmailFrom").Value!;
            _appSetting.SmtpHost = configuration.GetSection("AppSettings:SmtpHost").Value!;
            _appSetting.SmtpPort = int.Parse(configuration.GetSection("AppSettings:SmtpPort").Value!);
            _appSetting.SmtpUser = configuration.GetSection("AppSettings:SmtpUser").Value!;
            _appSetting.SmtpPass = configuration.GetSection("AppSettings:SmtpPass").Value!;
        }
        public void Send(string to, string subject, string html, string? from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _appSetting.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_appSetting.SmtpHost, _appSetting.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_appSetting.SmtpUser, _appSetting.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
