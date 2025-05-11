using Auth.Domain.Core.Data.DBEntity.Store;

namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class TransactionValidationRule(IRegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRule<Guid>(regex, action), ITransactionValidationRule
    {
        public override bool IsLengthFormatValid(Guid value)
        {
            if (value == Guid.Empty)
            {
                AddError(ErrorStatus.AccessDenied);
                return false;
            }
            return true;
        }
        public bool IsExist<T>(T value, Predicate<T> predicate = null)
        {
            if (value != null && predicate == null) return true;
            if (predicate(value)) return true;
            AddError(ErrorStatus.NotFound);
            return false;
        }
        public async Task<bool> IsExistAsync<T>(T id, Func<T, Task<bool>> isExistAsync)
        {
            if (await isExistAsync(id)) return true;
            AddError(ErrorStatus.NotFound);
            return false;
        }
        public async Task<bool> IsNotExistAsync<T>(T id, Func<T, Task<bool>> isExistAsync)
        {
            if (!await isExistAsync(id)) return true;
            AddError(ErrorStatus.AlreadyOccupied);
            return false;
        }
        public async Task<bool> IsHaveAccessAsync(Func<Guid,Task<bool>> isPendingAsync, Guid transactionId)
        {
            if (await isPendingAsync(transactionId))
                return true;
            AddError(ErrorStatus.AlreadyConfirmed, transactionId);
            return false;
        }
        public bool IsHaveAccess(ProductSubscription subscription, bool isActive)
        {
            if (subscription == null) return true;
            if (subscription.IsConfirmed) return true;
            if (!isActive) return true;
            AddError(ErrorStatus.AccessDenied);
            return false;
        }
        public bool IsContains(PaymentType type, params PaymentType[] availables)
        {
            if (availables.Contains(type)) return true;
            AddError(ErrorStatus.AccessDenied, type);
            return false;
        }
        public bool IsNotContains(PaymentType type, params PaymentType[] excludes)
        {
            if (!excludes.Contains(type)) return true;
            AddError(ErrorStatus.AccessDenied, type);
            return false;
        }
    }
}
