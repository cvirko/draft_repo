namespace Auth.Domain.Core.Logic.Commands.Payments
{
    public class UpdateWalletCommand : Command
    {
        public UpdateWalletCommand() { }
        public UpdateWalletCommand(Guid userId, decimal amount)
        {
            ToUserId = userId;
            Amount = amount;
        }
        public decimal Amount { get; set; }
        public Guid ToUserId { get; set; }
    }
}
