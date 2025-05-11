using Auth.Domain.Core.Common.Enums;

namespace Auth.Domain.Core.Data.Queues
{
    public record WaitingPaymentApproval(Guid TransactionId, 
        PaymentType Type, DateTime Expires, byte Attempt = 1);
}
