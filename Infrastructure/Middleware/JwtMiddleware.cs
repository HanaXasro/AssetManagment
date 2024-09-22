using Application.Exceptions;
using Infrastructure.DataContext;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context, DataDbContext dataContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachAccountToContext(context, dataContext, token);

            await _next(context);
        }

        private async Task attachAccountToContext(HttpContext context, DataDbContext dataContext, string token)
        {
            try
            {
                var Secretkey = configuration.GetSection("AppSettings:Secret");
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Secretkey.Value!);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                var resfreshToken = await dataContext.RefreshTokens.SingleOrDefaultAsync(x => x.UserId == userId && x.Expires > DateTime.UtcNow);

                if (resfreshToken == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid token.");
                    return;
                }

                var IpV4 = _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.MapToIPv4().ToString();
                if (!resfreshToken.CreatedByIp.Equals(IpV4))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Token not created by this IP.");
                    return;
                }

                context.Items["User"] = await dataContext.Users.Where(o => o.UserId == userId).
                    Include(o => o.RefreshTokens!.Where(re => re.Expires > DateTime.UtcNow)).SingleOrDefaultAsync();
            }
            catch
            {

            }
        }
    }
}
