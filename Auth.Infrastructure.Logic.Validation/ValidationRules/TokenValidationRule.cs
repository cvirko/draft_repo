namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class TokenValidationRule(RegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRule(regex, action), ITokenValidationRule
    {
        public override bool IsLengthFormatValid(string value)
        {
            if (IsLengthInvalid(value, nameof(UnitOfWorkValidationRule.Token)))
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
            if (token.Attempts > 0)
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
            var time = (token.CreationDate.AddMinutes(expiresTimeInMinutes) - DateTimeExtension.Get()).TotalSeconds;
            if (time > 10)
                return true;
            AddError(ErrorStatus.AccessDenied);
            return false;
        }
    }
}
