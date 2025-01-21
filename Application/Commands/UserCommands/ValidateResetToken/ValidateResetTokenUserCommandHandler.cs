using Application.Exceptions;
using Domain.Entities.UserEntity;
using MediatR;
using System.Linq.Expressions;

using Domain.Repositories.UserRepositories;

namespace Application.Commands.UserCommands.ValidateResetToken
{
    public class ValidateResetTokenUserCommandHandler(IUserRepository userRepository)
        : IRequestHandler<ValidateResetTokenUserCommand, string>
    {
        public async Task<string> Handle(ValidateResetTokenUserCommand request, CancellationToken cancellationToken)
        {

            Expression<Func<User, bool>> filter = x => x.ResetToken == request.token;
            var user = await userRepository.FindAsync(filter);
            if (user == null)
                throw new NotFoundException("Token Invalid.", request.token);

            if (user.ResetTokenExpires <= DateTime.UtcNow)
                throw new BadRequestException("Token Is Expired.");

            return "Token Is Activate";
        }
    }
}
