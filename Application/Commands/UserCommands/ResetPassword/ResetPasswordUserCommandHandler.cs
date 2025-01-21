using Domain.Entities.UserEntity;
using MediatR;
using System.Linq.Expressions;
using Application.Exceptions;
using Domain.Repositories.UserRepositories;
using BC = BCrypt.Net.BCrypt;

namespace Application.Commands.UserCommands.ResetPassword
{
    public class ResetPasswordUserCommandHandler(IUserRepository userRepository)
        : IRequestHandler<ResetPasswordUserCommand, string>
    {
        public async Task<string> Handle(ResetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> filter = x => x.ResetToken == request.Token;
            var user = await userRepository.FindAsync(filter);
            if (user == null)
                throw new NotFoundException("Token Invalid.", request.Token);

            await userRepository.ResetPassword(request.Token,BC.HashPassword(request.Password));
            return "Reset Password Successfully.";
        }
    }
}
