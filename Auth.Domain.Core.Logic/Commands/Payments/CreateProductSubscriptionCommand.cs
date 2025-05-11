namespace Auth.Domain.Core.Logic.Commands.Payments
{
    public class CreateProductSubscriptionCommand : Command
    {
        public uint ProductId { get; set; }
        public PaymentType Type { get; set; }
        public PaymentCurrency Currency { get; set; }
    }
}
