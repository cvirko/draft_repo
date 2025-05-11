using Auth.Domain.Core.Data.DBEntity.Account;
using Auth.Domain.Core.Data.Extensions;

namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class TokenValidationRule(IRegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRule<string>(regex, action), ITokenValidationRule
    {
        public override bool IsLengthFormatValid(string value)
        {
            if (IsLengthInvalid(value, new Range(6,50)))
                return false;

            return true;
        }
        public bool IsOnlyNumbers(string value)
        {
            if (_regex.IsNotOnlyNumbers(value))
            {
                AddError(ErrorStatus.Format);
                return false;
            }
            return true;
        }
        public bool IsMatch(string value, UserToken token)
        {
            if (token == null)
            {
                AddError(ErrorStatus.NotFound);
                return false;
            }
            if (value == token.Token)
                return true;
            AddError(ErrorStatus.Invalid, token.Attempts-1);
            if (token.Attempts == 1)
                AddError(ErrorStatus.NoAttempts);
            return false;
        }
        public bool IsHaveAttempts(UserToken token, float expiresTimeInMinutes)
        {
            if (!IsNotExpiraced(token, expiresTimeInMinutes))
                return false;
            if (token.IsHaveAttempts())
                return true;
            AddError(ErrorStatus.NoAttempts);
            return false;
        }
        public bool IsNotExpiraced(UserToken token, float expiresTimeInMinutes)
        {
            if (token == null)
            {
                AddError(ErrorStatus.NotFound);
                return false;
            }
            if (token.IsConfirmed)
            {
                AddError(ErrorStatus.AlreadyConfirmed);
                return false;
            }
            var expireInSeconds = (token.DateAt.AddMinutes(expiresTimeInMinutes) - DateTimeExtension.Get()).TotalSeconds;
            if (expireInSeconds > 10)
                return true;
            AddError(ErrorStatus.AccessDenied);
            return false;
        }
    }
}
