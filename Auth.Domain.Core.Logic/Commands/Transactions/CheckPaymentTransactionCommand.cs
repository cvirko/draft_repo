using Auth.Domain.Core.Logic.CommandsResponse.Payments;

namespace Auth.Domain.Core.Logic.Commands.Transactions
{
    public class CheckPaymentTransactionCommand : Command<CheckPaymentTransactionResponse>
    {
        public CheckPaymentTransactionCommand(){}
        public CheckPaymentTransactionCommand(Guid transactionId)
        {
            TransactionId = transactionId;
        }
        public Guid? TransactionId { get; set; }
    }
}
