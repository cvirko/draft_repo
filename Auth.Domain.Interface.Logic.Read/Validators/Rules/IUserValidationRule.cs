using Auth.Domain.Core.Data.DBEntity.Account;

namespace Auth.Domain.Interface.Logic.Read.Validators.Rules
{
    public interface IUserValidationRule : IValidationRule<Guid>
    {
        bool IsHaveAccess(User user);
        bool IsNotEqual<T>(T value, T newValue);
        bool IsExist<T>(T value);
    }
}
