namespace Auth.Domain.Core.Logic.CommandsResponse.Payments
{
    public class PayForOrderResponse 
    {
        public string UserSecret { get; set; }
        public string PaymentId { get; set; }
        public Guid TransactionId { get; set; }
    }
}
