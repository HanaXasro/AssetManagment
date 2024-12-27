using Application.Exceptions;
using Application.Helper;
using Domain.Entities.UserEntity;
using Domain.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories.UserRepositories;

namespace Application.Commands.UserCommands.ResendCodeToRest
{
    public class ResendCodeToRestPasswordCH(
        IUserRepository userRepository,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<ResendCodeToRestPasswordC, string>
    {
        public async Task<string> Handle(ResendCodeToRestPasswordC request, CancellationToken cancellationToken)
        {

            Expression<Func<User, bool>> filter = x => x.Email == request.Email;

            var userExist = await userRepository.FindAsync(filter);
            if (userExist == null || userExist.ResetTokenExpires == null)
                throw new NotFoundException("Email not Found.", request.Email);

            if (userExist.ResetTokenExpires < DateTime.UtcNow)
            {
                var code4D = new Random().Next(0000, 9999).ToString("D4");
                var userRest = await userRepository.ResendCodeToRest(userExist.UserId, code4D);
                sendPasswordResetEmail(userRest!);
            }

            sendPasswordResetEmail(userExist);

            return "Send Code to Email Successfully";

        }

        private void sendPasswordResetEmail(User user)
        {
            string message;
            string origin = httpContextAccessor.HttpContext!.Request!.Headers["origin"]!;
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
