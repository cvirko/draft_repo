namespace Auth.Domain.Interface.Logic.Read.Validators.Rules
{
    public interface IUserValidationRule : IValidationRule
    {
        bool IsHaveAccess(User user);
        bool IsNotEqual<T>(T value, T newValue);
        bool IsExist<T>(T value);
    }
}
