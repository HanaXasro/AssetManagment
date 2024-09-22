using Application.Helper;
using Domain.Repositories.UserRepositores;
using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using Domain.Entities.UserEntity;

namespace Application.Commands.UserCommands.RevokeToken
{
    public class RevokeTokenUserCommandHandler : IRequestHandler<RevokeTokenUserCommand, string>
    {

        private readonly IUserRepository userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserReference _userReference;

        public RevokeTokenUserCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IUserReference userReference)
        {
            this.userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _userReference = userReference;
        }


        public async Task<string> Handle(RevokeTokenUserCommand request, CancellationToken cancellationToken)
        {

            var userId = _userReference.UserId;
            Expression<Func<Domain.Entities.UserEntity.RefreshToken, bool>> filter = o => o.Token == request.Token;

            var refreshTokenExist = await userRepository.FindAsync(filter);
            if (refreshTokenExist == null)
                throw new NotFoundEx("Token not found.", request.Token);

            if (!refreshTokenExist.IsActive)
                throw new BadRequestEx("Token Invalid or Expired.");

            var refreshToken = await userRepository.RevokeToken(request.Token, ipAddressV4());
            DelTokenCookie();
            return "Token Is Revoked Successfuly.";

        }

        private string ipAddressV4()
        {
            if (_httpContextAccessor.HttpContext!.Request.Headers.ContainsKey("X-Forwarded-For"))
                return _httpContextAccessor.HttpContext!.Request.Headers["X-Forwarded-For"]!;
            else
                return _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.MapToIPv4().ToString();
        }

        private void DelTokenCookie()
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", "");
        }
    }
}
