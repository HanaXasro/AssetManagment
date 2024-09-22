using Application.Exceptions;
using Application.Helper;
using Domain.Entities.UserEntity;
using Domain.Repositories.UserRepositores;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCommands.VerifyEmail
{
    public class VerifyEmailUserCommandHandler : IRequestHandler<VerifyEmailUserCommand, string>
    {
        private readonly IUserRepository userRepository;

        public VerifyEmailUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<string> Handle(VerifyEmailUserCommand request, CancellationToken cancellationToken)
        {

            Expression<Func<User, bool>> filter = x => x.VerificationToken == request.Token;
            var user = await userRepository.FindAsync(filter);
            if (user == null)
                throw new NotFoundEx("Code Invalid.", request.Token);
            await userRepository.VerifyEmail(request.Token);

            return "Verify Email Successfuly.";
        }
    }
}
