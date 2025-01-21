using AutoMapper;
using Domain.Service;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using Application.Exceptions;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Domain.Entities.UserEntity;
using Domain.Repositories.UserRepositories;

namespace Application.Commands.UserCommands.Register
{
    public class RegisterUserCommandHandler(
        IMapper mapper,
        IUserRepository userRepository,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<RegisterUserCommand, string>
    {
        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> filter = x => x.Email == request.Email;

            var isUniqueEmail = await userRepository.FindAsync(filter);

            if (isUniqueEmail != null)
            {
                await userRepository.ForgotPassword(request.Email, CreateRefreshToken());
                SendAlreadyRegisteredEmail(request.Email);
                throw new BadRequestException("Email Already Exists, check your Email to Forgot Password.");
            }

            Expression<Func<User, bool>> filterUsername = x => x.Username == request.Username;
            var isUniqueUsername = await userRepository.FindAsync(filterUsername);

            if (isUniqueUsername != null)
            {
                throw new BadRequestException($"Username not Available ({request.Username}).");
            }

            var accountEntity = mapper.Map<User>(request);
            accountEntity.PasswordHash = BC.HashPassword(request.Password);
            accountEntity.VerificationToken = new Random().Next(0000, 9999).ToString("D4");
            accountEntity.Created = DateTime.UtcNow;
            var user = await userRepository.Register(accountEntity);
            SendVerificationEmail(user);
            return "Register Successfuly, to Verify Ckeck Your Email.";
        }

        private string CreateRefreshToken()
        {
            string refreshToken = Guid.NewGuid().ToString("N").ToUpper() +
                Guid.NewGuid().ToString("N").ToUpper() +
                Guid.NewGuid().ToString("N").ToUpper();
            return refreshToken;
        }

        private void SendAlreadyRegisteredEmail(string email)
        {
            string message;
            string origin = httpContextAccessor.HttpContext!.Request!.Headers["origin"]!;
            if (!string.IsNullOrEmpty(origin))
                message = $@"<p>If you don't know your password please visit the <a href=""{origin}/user/forgot-password"">forgot password</a> page.</p>";
            else
                message = "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";

            emailService.Send(
                to: email,
                subject: "Sign-up Verification API - Email Already Registered",
                html: $@"<h4>Email Already Registered</h4>
                         <p>Your email <strong>{email}</strong> is already registered.</p>
                         {message}"
            );
        }

        private void SendVerificationEmail(User user)
        {
            string message;
            string origin = httpContextAccessor.HttpContext!.Request!.Headers["origin"]!;
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
