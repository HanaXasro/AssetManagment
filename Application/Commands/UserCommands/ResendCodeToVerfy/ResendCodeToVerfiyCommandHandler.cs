using Application.Exceptions;
using Application.Helper;
using Domain.Entities.UserEntity;
using Domain.Repositories.UserRepositores;
using Domain.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCommands.ResendCodeToVerfy
{
    public class ResendCodeToVerfiyCommandHandler : IRequestHandler<ResendCodeToVerfiyCommand, string>
    {

        private readonly IUserRepository userRepository;
        private readonly IEmailService emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ResendCodeToVerfiyCommandHandler(IUserRepository userRepository, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Handle(ResendCodeToVerfiyCommand request, CancellationToken cancellationToken)
        {

            Expression<Func<User, bool>> filter = x => x.Email == request.email; 

            var userExist = await userRepository.FindAsync(filter);
            if (userExist == null || userExist.VerificationToken == null)
                throw new NotFoundEx("email not Found.",request.email);

            var code = new Random().Next(0000, 9999).ToString("D4");
            var user = await userRepository.ResendCodeToVerfy(userExist.UserId, code);
            sendVerificationEmail(user);

            return "Send Code to Email Successfully";

        }


        private void sendVerificationEmail(User user)
        {
            string message;
            string origin = _httpContextAccessor.HttpContext!.Request!.Headers["origin"]!;
            if (!string.IsNullOrEmpty(origin))
            {
                // var verifyUrl = $"{origin}/user/verify-email?token={user.VerificationToken}";
                // <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
                message = $"<p style=\"font-size: 18px; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;\">" +
                    $"Your Verification code Is:</p> <div>" +
                    $"<div style=\"margin-top: 30px; display: flex; justify-content: center;\"><span  style=\"padding-top: 20px;" +
                    $"padding-bottom: 20px; padding-left: 10px;padding-right: 10px; border-radius: 5px;border: 2px solid #C7C8CC; text-align: center;" +
                    $"font-family: Arial, Helvetica, sans-serif; font-size: 16px;\">{user.VerificationToken}</span></div></div>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p><code>{user.VerificationToken}</code></p>";
            }

            emailService.Send(
                to: user.Email,
                subject: "Sign-up Verification API - Verify Email",
                html: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
            );
        }
    }
}
