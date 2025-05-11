using Auth.Domain.Core.Data.DBEntity.Account;
using Auth.Domain.Interface.Data.Read.Repository;

namespace Auth.Infrastructure.Data.Repository
{
    internal class UserRepository<T>(T dbContext) : IUserRepository where T : BaseDBContext
    {
        private readonly T _context = dbContext;

        public async Task<bool> IsExistEmailAsync(string email)
        {
            return await _context.UsersLogin
                .AnyAsync(p => p.Email.Equals(email.ToLower()));
        }

        public async Task<UserLogin> GetLoginByEmailAsync(string email)
        {
            return await _context.UsersLogin
                .Where(p => p.Email == email)
                .Include(p => p.User)
                .FirstOrDefaultAsync();
        }
        public async Task<UserLogin> GetLoginByUserIdAsync(Guid userId, ulong loginId)
        {
            return await _context.UsersLogin
                .Where(p => p.LoginId == loginId && p.UserId == userId)
                .Include(p => p.User)
                .FirstOrDefaultAsync();
        }
        public async Task<UserToken[]> GetUserTokensAsync(Guid userId)
        {
            return await _context.UsersTokens
                .Where(p => p.UserId == userId).ToArrayAsync();
        }
        public async Task<UserToken> GetUserTokenAsync(string userInfo, Guid userId, TokenType type)
        {
            return await _context.UsersTokens.FindAsync(userId, type, userInfo);
        }
        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        public async Task<bool> IsExistUserAsync(Guid userId)
        {
            return await _context.Users
                .AnyAsync(p => p.UserId == userId);
        }
        public async Task<UserWallet> GetWalletAsync(Guid userId)
        {
            return await _context.Wallets
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
        public async Task<decimal?> GetExpectedBalanceAsync(Guid userId, decimal amount)
        {
            return await _context.Wallets
                .Where(p => p.UserId == userId)
                .Select(p => p.Balance + amount)
                .FirstOrDefaultAsync();
        }
    }
}
