using Application.Helper;
using Domain.Service;
using Domain.Entities.UserEntity;
using Domain.Repositories.UserRepositores;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace Application.Commands.UserCommands.ForgotPassword
{
    public class ForgotPasswordUserCommandHandler : IRequestHandler<ForgotPasswordUserCommand, string>
    {
        private readonly IUserRepository userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService emailService;

        public ForgotPasswordUserCommandHandler(IUserRepository userRepository, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Handle(ForgotPasswordUserCommand request, CancellationToken cancellationToken)
        {

            Expression<Func<User, bool>> filter = x => x.ResetToken == request.Email;
            var userFind = await userRepository.FindAsync(filter);
            if (userFind == null)
                throw new NotFoundEx("Email Not Found.", request.Email);

            var user = await userRepository.ForgotPassword(request.Email, new Random().Next(0000, 9999).ToString("D4"));
            sendPasswordResetEmail(user);
            return "Token Send Email Please Check Your Email.";
        }


        private void sendPasswordResetEmail(User user)
        {
            string message;
            string origin = _httpContextAccessor.HttpContext!.Request!.Headers["origin"]!;
            if (!string.IsNullOrEmpty(origin))
            {
                // var resetUrl = $"{origin}/user/reset-password?token={user.ResetToken}";
                //message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                //             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
                message = $"<p style=\"font-size: 18px; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;\">" +
                   $"Your Verification code Is:</p>" +
                   $"<div style=\"margin-top: 30px; display: flex; justify-content: center;\"><span  style=\"padding-top: 20px;" +
                   $"padding-bottom: 20px; padding-left: 10px;padding-right: 10px; border-radius: 5px;border: 2px solid #C7C8CC; text-align: center;" +
                   $"font-family: Arial, Helvetica, sans-serif; font-size: 16px;\">{user.ResetToken}</span></div>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                             <p><code>{user.ResetToken}</code></p>";
            }

            emailService.Send(
                to: user.Email,
                subject: "Sign-up Verification API - Reset Password",
                html: $@"<h4>Reset Password Email</h4>
                         {message}"
            );
        }

    }
}
