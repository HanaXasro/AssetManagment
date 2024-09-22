using AutoMapper;
using Application.Dtos.AccountDtos;
using Application.Exceptions;
using Application.Helper;
using Domain.Entities.UserEntity;
using Domain.Repositories.UserRepositores;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Application.Commands.UserCommands.RefreshToken
{
    public class RefreshTokenUserCommandHandler : IRequestHandler<RefreshTokenUserCommand, LoginReqponseDto>
    {

        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper mapper;

        public RefreshTokenUserCommandHandler(IUserRepository userRepository, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<LoginReqponseDto> Handle(RefreshTokenUserCommand request, CancellationToken cancellationToken)
        {

            Expression<Func<Domain.Entities.UserEntity.RefreshToken, bool>> filter = x => x.Token == request.Token;

            var refreshTokenExists = await userRepository.FindAsync(filter);
            if (refreshTokenExists == null)
                throw new NotFoundEx("Referesh token not found.", request.Token);

            if (!refreshTokenExists.IsActive)
                throw new BadRequestEx("Token Is Expired.");

            var refreshToken = new Domain.Entities.UserEntity.RefreshToken()
            {
                Token = CreateRefreshToken(),
                User = refreshTokenExists.User,
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddressV4(),
                Expires = DateTime.UtcNow.AddMinutes(15),
            };

            var revokeTokenResult = await userRepository.RevokeToeknAndReplase(refreshTokenExists.Token,
                refreshToken.CreatedByIp, refreshToken.Token);

            if (revokeTokenResult == null)
                throw new BadRequestEx("Revoke Token Filed.");

            var createRefreshtokenResult = await userRepository.CreateRefreshAsync(refreshToken);

            var Result = new LoginReqponseDto()
            {
                user = mapper.Map<AccountDto>(refreshTokenExists.User),
                Token = CreateToken(refreshToken.User!),
                RefreshToken = refreshToken.Token,
                Expired = DateTime.UtcNow.AddMinutes(15)
            };
            setTokenCookie(refreshToken.Token);

            return Result;
        }

        private string CreateToken(User user)
        {
            var Secretkey = configuration.GetSection("AppSettings:Secret");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secretkey.Value!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            string refreshToken = Guid.NewGuid().ToString("N").ToUpper() +
                Guid.NewGuid().ToString("N").ToUpper() +
                Guid.NewGuid().ToString("N").ToUpper();
            return refreshToken;
        }

        private string ipAddressV4()
        {
            if (_httpContextAccessor.HttpContext!.Request.Headers.ContainsKey("X-Forwarded-For"))
                return _httpContextAccessor.HttpContext!.Request.Headers["X-Forwarded-For"]!;
            else
                return _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.MapToIPv4().ToString();
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(2)
            };
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
