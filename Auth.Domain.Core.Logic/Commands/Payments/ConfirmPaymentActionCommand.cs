namespace Auth.Domain.Core.Logic.Commands.Payments
{
    public class ConfirmPaymentActionCommand : Command
    {
        public string PaymentId { get; set; }
        public Guid TransactionId { get; set; }
        public PaymentType Type { get; set; }
        public ActionStatus Status { get; set; }
    }
}
