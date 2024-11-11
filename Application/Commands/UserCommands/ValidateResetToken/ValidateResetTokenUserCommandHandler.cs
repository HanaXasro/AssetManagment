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

namespace Application.Commands.UserCommands.ValidateResetToken
{
    public class ValidateResetTokenUserCommandHandler : IRequestHandler<ValidateResetTokenUserCommand, string>
    {

        private readonly IUserRepository userRepository;

        public ValidateResetTokenUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        public async Task<string> Handle(ValidateResetTokenUserCommand request, CancellationToken cancellationToken)
        {

            Expression<Func<User, bool>> filter = x => x.ResetToken == request.token;
            var user = await userRepository.FindAsync(filter);
            if (user == null)
                throw new NotFoundEx("Token Invalid.", request.token);

            if (user.ResetTokenExpires <= DateTime.UtcNow)
                throw new BadRequestEx("Token Is Exiered.");

            return "Token Is Activate";
        }
    }
}
