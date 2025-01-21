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

namespace Application.Commands.UserCommands.VerifyEmail
{
    public class VerifyEmailUserCommandHandler(IUserRepository userRepository)
        : IRequestHandler<VerifyEmailUserCommand, string>
    {
        public async Task<string> Handle(VerifyEmailUserCommand request, CancellationToken cancellationToken)
        {

            Expression<Func<User, bool>> filter = x => x.VerificationToken == request.Token;
            var user = await userRepository.FindAsync(filter);
            if (user == null)
                throw new NotFoundException("Code Invalid.", request.Token);
            await userRepository.VerifyEmail(request.Token);

            return "Verify Email Successfully.";
        }
    }
}
