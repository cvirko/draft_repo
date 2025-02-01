namespace Auth.Domain.Interface.Logic.Read.Validators.Rules
{
    public interface ITokenValidationRule : IValidationRule
    {
        bool IsMatch(string value, UserToken token);
        bool IsOnlyNumbers(string value);
        bool IsHaveAttempts(UserToken token, float expiresTimeInMinutes);
        bool IsNotExpiraced(UserToken token, float expiresTimeInMinutes);
    }
}
