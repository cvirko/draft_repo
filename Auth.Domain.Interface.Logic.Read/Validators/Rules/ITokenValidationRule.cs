using Auth.Domain.Core.Data.DBEntity.Account;

namespace Auth.Domain.Interface.Logic.Read.Validators.Rules
{
    public interface ITokenValidationRule : IValidationRule<string>
    {
        bool IsMatch(string value, UserToken token);
        bool IsOnlyNumbers(string value);
        bool IsHaveAttempts(UserToken token, float expiresTimeInMinutes);
        bool IsNotExpiraced(UserToken token, float expiresTimeInMinutes);
    }
}
