using Auth.Domain.Core.Data.DBEntity.Account;

namespace Auth.Domain.Interface.Logic.Read.Validators.Rules
{
    public interface IEmailValidationRule : IValidationRule<string>
    {
        Task<bool> IsNotOccupiedAsync(string value, Func<string, Task<bool>> queryAsync);
        Task<bool> IsExistAsync(string value, Func<string, Task<bool>> queryAsync);
        bool IsExist(UserLogin value);
        bool IsDelayGone(string email, DateTime? lastMailDate, double delayBetweenMessagesInMinutes = 1);
    }
}
