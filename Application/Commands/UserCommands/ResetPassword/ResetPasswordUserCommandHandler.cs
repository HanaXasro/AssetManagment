using Application.Exceptions;
using Application.Helper;
using Domain.Entities.UserEntity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories.UserRepositories;
using BC = BCrypt.Net.BCrypt;

namespace Application.Commands.UserCommands.ResetPassword
{
    public class ResetPasswordUserCommandHandler : IRequestHandler<ResetPasswordUserCommand, string>
    {
        private readonly IUserRepository userRepository;

        public ResetPasswordUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        public async Task<string> Handle(ResetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> filter = x => x.ResetToken == request.Token;
            var user = await userRepository.FindAsync(filter);
            if (user == null)
                throw new NotFoundException("Token Invalid.", request.Token);

            await userRepository.ResetPassword(request.Token,BC.HashPassword(request.Password));
            return "Reset Password Successfuly.";
        }
    }
}
