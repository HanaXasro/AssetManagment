using AutoMapper;
using Application.Dtos.AccountDtos;
using Application.Exceptions;
using Application.Helper;
using Domain.Entities.UserEntity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Repositories.UserRepositories;
using BC = BCrypt.Net.BCrypt;

namespace Application.Commands.UserCommands.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginReqponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginUserCommandHandler(IMapper mapper, IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        { 
            _mapper = mapper;
            _userRepository = userRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginReqponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.Login(request.UsernameOrEmail);
            if (user == null || !BC.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedException("Email or Passsword Incorect.", request.UsernameOrEmail);

            if (string.IsNullOrWhiteSpace(user.Verified.ToString()) || !string.IsNullOrWhiteSpace(user.VerificationToken))
                throw new BadRequestException("You are not Verifiy.");


            var Result = new LoginReqponseDto()
            {
                user = _mapper.Map<UserDto>(user),
                Token = CreateToken(user),
                RefreshToken = CreateRefreshToken(),
                Expired = DateTime.UtcNow.AddMinutes(15)
            };

            var refreshToken = new Domain.Entities.UserEntity.RefreshToken()
            {
                Token = Result.RefreshToken,
                User = user,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(15),
                CreatedByIp = ipAddressV4()
            };
            setTokenCookie(refreshToken.Token, _httpContextAccessor.HttpContext!.Response!);
            await _userRepository.CreateRefreshAsync(refreshToken);

            return Result;
        }

        private string CreateToken(User user)
        {
            var Secretkey = _configuration.GetSection("AppSettings:Secret");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secretkey.Value!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(20),
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

        private void setTokenCookie(string token, HttpResponse Response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
