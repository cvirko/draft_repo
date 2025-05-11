namespace Auth.Domain.Core.Logic.CommandsResponse.Payments
{
    public class SubscriptionResponse 
    {
        public string PaymentId { get; set; }
        public Guid TransactionId { get; set; }
    }
}
