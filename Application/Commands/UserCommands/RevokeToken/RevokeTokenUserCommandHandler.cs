using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using Domain.Repositories.UserRepositories;

namespace Application.Commands.UserCommands.RevokeToken
{
    public class RevokeTokenUserCommandHandler : IRequestHandler<RevokeTokenUserCommand, string>
    {

        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserReference _userReference;

        public RevokeTokenUserCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IUserReference userReference)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _userReference = userReference;
        }


        public async Task<string> Handle(RevokeTokenUserCommand request, CancellationToken cancellationToken)
        {

            var userId = _userReference.UserId;
            Expression<Func<Domain.Entities.UserEntity.RefreshToken, bool>> filter = o => o.Token == request.Token;

            var refreshTokenExist = await _userRepository.FindAsync(filter);
            if (refreshTokenExist == null)
                throw new NotFoundException("Token not found.", request.Token);

            if (!refreshTokenExist.IsActive)
                throw new BadRequestException("Token Invalid or Expired.");

            var refreshToken = await _userRepository.RevokeToken(request.Token, IpAddressV4());
            DelTokenCookie();
            return "Token Is Revoked Successfuly.";

        }

        private string IpAddressV4()
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
