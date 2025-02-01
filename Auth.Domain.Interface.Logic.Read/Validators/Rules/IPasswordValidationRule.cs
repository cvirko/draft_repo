namespace Auth.Domain.Interface.Logic.Read.Validators.Rules
{
    public interface IPasswordValidationRule : IValidationRule
    {
        bool IsMatch(UserLogin login, string value, Func<Guid, string, string, bool> query);
        bool IsMatch(Guid userId, string passwordHash, string value, int attempts, Func<Guid, string, string, bool> query);
        bool IsHaveAttempts(UserLogin login);
        bool IsNotBanned(UserLogin login);
    }
}
