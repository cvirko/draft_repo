namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class PasswordValidationRule(RegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRule(regex, action), IPasswordValidationRule
    {
        public override bool IsLengthFormatValid(string value)
        {
            if (IsLengthInvalid(value, nameof(UnitOfWorkValidationRule.Password)))
                return false;

            if (_regex.IsPasswordInvalidFormat(value))
            {
                AddError(ErrorStatus.Format);
                return false;
            }
            return true;
        }
        public bool IsMatch(UserLogin login, string value, Func<Guid, string, string, bool> query)
        {
            return IsMatch(login.UserId, login.PasswordHash, value, login.Attempts,query);
        }
        public bool IsMatch(Guid userId, string passwordHash, string value, int attempts, Func<Guid,string, string, bool> query)
        {
            if (query(userId, passwordHash, value))
                return true;
            AddError(ErrorStatus.Invalid, attempts-1);
            if (attempts == 1)
                AddError(ErrorStatus.NoAttempts);
            return false;
        }
        public bool IsHaveAttempts(UserLogin login)
        {
            if (login.Attempts > 0)
                return true;
            AddError(ErrorStatus.NoAttempts);
            return false;
        }
        public bool IsNotBanned(UserLogin login)
        {
            if (login?.BanExpireDate != null)
            {
                var time = (login.BanExpireDate.Value - DateTimeExtension.Get()).TotalSeconds;
                if (time < 10)
                {
                    AddError(ErrorStatus.AccessDenied, login.BanExpireDate);
                    return false;
                }
                
            }
            
            return false;
        }
    }
}
