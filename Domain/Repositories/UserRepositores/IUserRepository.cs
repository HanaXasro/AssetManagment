using Domain.Entities.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.UserRepositores
{
    public interface IUserRepository
    {
        Task<User> Register(User user);
        Task<User?> Login(string usernameOrEmail);
        Task<User> VerifyEmail(string token);
        Task<User> ForgotPassword(string email,string token);
        Task<User?> IsUnique(Expression<Func<User , bool>> query);
        Task<RefreshToken> CreateRefreshAsync(RefreshToken refreshToken);
        Task<RefreshToken?> RevokeToeknAndReplase(string Token, string IpAddressV4,string ReplaseToken);
        Task<RefreshToken> RevokeToekn(string Token,string IpAddressV4);
        Task<User> ResetPassword(string token, string password);
        Task<RefreshToken> RevokeToken(string token,string IpAddressV4);
        Task<User> ResendCodeToRest(Guid UserId,string token);
        Task<User> ResendCodeToVerfy(Guid UserId,string token);

        Task<User?> FindAsync(Expression<Func<User, bool>> query);
        Task<RefreshToken?> FindAsync(Expression<Func<RefreshToken, bool>> query);

    }
}
