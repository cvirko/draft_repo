namespace Auth.Domain.Core.Logic.Commands.Transactions
{
    public class AddPaymentTransactionCommand : Command<Guid>
    {
        public AddPaymentTransactionCommand(){}
        public AddPaymentTransactionCommand(
            Guid userId,
            PaymentType type)
        {
            Type = type;
            UserId = userId;
        }
        public PaymentType Type { get; set; }
    }
}
