using Domain.Entities.UserEntity;
using Domain.Repositories.UserRepositores;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.AccountRepositories
{
    public class UserRepository : IUserRepository
    {

        private readonly DataDbContext context;

        public UserRepository(DataDbContext context)
        {
            this.context = context;
        }

        public async Task<RefreshToken> CreateRefreshAsync(RefreshToken refreshToken)
        {
            var olRefreshToken = await context.RefreshTokens.Where(o => o.User!.UserId == refreshToken.User!.UserId).ToListAsync();
            olRefreshToken.ForEach(o => o.Revoked = DateTime.UtcNow);
            olRefreshToken.ForEach(o => o.RevokedByIp = refreshToken.CreatedByIp);
            olRefreshToken.ForEach(o => o.Expires = DateTime.UtcNow);
            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<User?> FindAsync(Expression<Func<User, bool>> query)
        {
            return await context.Users.FirstOrDefaultAsync(query);
        }

        public async Task<User> ForgotPassword(string email, string token)
        {
            var user = await context.Users.FirstOrDefaultAsync(o => o.Email == email);
            user!.ResetToken = token;
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15);
            await context.SaveChangesAsync();
            return user;

        }

        public async Task<RefreshToken?> FindAsync(Expression<Func<RefreshToken, bool>> query)
        {
            return await context.RefreshTokens.Include(o => o.User).FirstOrDefaultAsync(query);
        }

        public async Task<User?> IsUnique(Expression<Func<User, bool>> query)
        {
            var user = await context.Users.SingleOrDefaultAsync(query);
            return user;
        }

        public async Task<User?> Login(string usernameOrEmail)
        {
            var user = await context.Users.FirstOrDefaultAsync(o => o.Email == usernameOrEmail || o.Username == usernameOrEmail);
            return user;
        }

        public async Task<User> Register(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;

        }


        public async Task<User> ResendCodeToRest(Guid UserId, string token)
        {
            var user = await context.Users.FirstOrDefaultAsync(o => o.UserId == UserId);
            user!.VerificationToken = token;
            user.ResetTokenExpires = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> ResendCodeToVerfy(Guid UserId, string token)
        {
            var user = await context.Users.FirstOrDefaultAsync(o => o.UserId == UserId);
            user!.VerificationToken = token;
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> ResetPassword(string token, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(o => o.ResetToken == token);
            user!.ResetToken = null;
            user.PasswordHash = password;
            user.Updated = DateTime.UtcNow;
            user.ResetTokenExpires = null;
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<RefreshToken> RevokeToekn(string Token, string IpAddressV4)
        {
            var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(o => o.Token == Token);
            refreshToken!.Expires = DateTime.UtcNow;
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = IpAddressV4;
            await context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken?> RevokeToeknAndReplase(string Token, string IpAddressV4, string ReplaseToken)
        {
            var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(o => o.Token == Token);

            refreshToken!.Expires = DateTime.UtcNow;
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = IpAddressV4;
            refreshToken.ReplacedByToken = ReplaseToken;
            await context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken> RevokeToken(string token, string IpAddressV4)
        {
            var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(o => o.Token == token &&
            o.Expires > DateTime.UtcNow);

            refreshToken!.Expires = DateTime.UtcNow;
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.ReplacedByToken = IpAddressV4;
            await context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<User> VerifyEmail(string token)
        {
            var user = await context.Users.FirstOrDefaultAsync(o => o.VerificationToken == token);
            user!.VerificationToken = null;
            user.Verified = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return user;
        }
    }
}
