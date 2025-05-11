using Auth.Domain.Core.Logic.CommandsResponse.Payments;

namespace Auth.Domain.Core.Logic.Commands.Payments
{
    public class PayForOrderCommand : Command<PayForOrderResponse>
    {
        public PaymentType Type { get; set; }
        public PaymentCurrency Currency { get; set; }
    }
}
