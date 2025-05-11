namespace Auth.Domain.Core.Data.DBEntity.Account
{
    public class UserWallet : TEntity
    {
        public uint WalletId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public decimal Balance { get; set; }
        
    }
}
