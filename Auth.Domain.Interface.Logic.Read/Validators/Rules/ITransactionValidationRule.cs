using Auth.Domain.Core.Data.DBEntity.Store;

namespace Auth.Domain.Interface.Logic.Read.Validators.Rules
{
    public interface ITransactionValidationRule : IValidationRule<Guid>
    {
        Task<bool> IsHaveAccessAsync(Func<Guid, Task<bool>> isPendingAsync, Guid transactionId);
        bool IsHaveAccess(ProductSubscription subscription, bool isActive);
        bool IsExist<T>(T value, Predicate<T> predicate = null);
        Task<bool> IsExistAsync<T>(T id, Func<T, Task<bool>> isExistAsync);
        Task<bool> IsNotExistAsync<T>(T id, Func<T, Task<bool>> isExistAsync);
        bool IsContains(PaymentType type, params PaymentType[] availables);
        bool IsNotContains(PaymentType type, params PaymentType[] excludes);
    }
}
