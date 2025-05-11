using Auth.Domain.Core.Logic.CommandsResponse.Payments;

namespace Auth.Domain.Core.Logic.Commands.Payments
{
    public class SubscriptionCommand : Command<SubscriptionResponse>
    {
        public uint ProductId { get; set; }
        public PaymentType Type { get; set; }
        public string CustomerId { get; set; }
        public string PaymentMethodId { get; set; }
    }
}
