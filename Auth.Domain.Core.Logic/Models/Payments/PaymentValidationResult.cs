namespace Auth.Domain.Core.Logic.Models.Payments
{
    public record PaymentValidationResult(
        ActionStatus Status, PaymentType Type, string Id, Guid TransactionId);
}
