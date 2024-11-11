using System.Linq.Expressions;
using Domain.Entities.UserEntity;

namespace Domain.Repositories.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> Register(User user);
        Task<User?> Login(string usernameOrEmail);
        Task<User> VerifyEmail(string token);
        Task<User> ForgotPassword(string email,string token);
        Task<User?> IsUnique(Expression<Func<User , bool>> query);
        Task<RefreshToken> CreateRefreshAsync(RefreshToken refreshToken);
        Task<RefreshToken?> RevokeTokenAndRelapse(string token, string ipAddressV4,string replaseToken);
        Task<User> ResetPassword(string token, string password);
        Task<RefreshToken> RevokeToken(string token,string ipAddressV4);
        Task<User> ResendCodeToRest(Guid userId,string token);
        Task<User> ResendCodeToVerify(Guid userId,string token);

        Task<User?> FindAsync(Expression<Func<User, bool>> query);
        Task<RefreshToken?> FindAsync(Expression<Func<RefreshToken, bool>> query);

    }
}
