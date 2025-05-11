using Auth.Domain.Core.Common.Enums;
using Auth.Domain.Core.Data.DBEntity.Account;

namespace Auth.Domain.Interface.Data.Read.Repository
{
    public interface IUserRepository
    {
        Task<bool> IsExistEmailAsync(string email);
        Task<UserLogin> GetLoginByUserIdAsync(Guid userId, ulong loginId);
        Task<UserLogin> GetLoginByEmailAsync(string email);
        Task<UserToken> GetUserTokenAsync(string userInfo, Guid userId, TokenType type);
        Task<User> GetUserAsync(Guid userId);
        Task<bool> IsExistUserAsync(Guid userId);
        Task<UserToken[]> GetUserTokensAsync(Guid userId);
        Task<UserWallet> GetWalletAsync(Guid userId);
        Task<decimal?> GetExpectedBalanceAsync(Guid userId, decimal amount);
    }
}
