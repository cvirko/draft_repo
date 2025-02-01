namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class UserValidationRule(RegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRule(regex, action), IUserValidationRule
    {
        public override bool IsLengthFormatValid(string value)
        {
            if (IsLengthInvalid(value, nameof(UnitOfWorkValidationRule.User)))
                return false;

            if (value == Guid.Empty.ToString())
            {
                AddError(ErrorStatus.AccessDenied);
                return false;
            }
            return true;
        }
        public bool IsHaveAccess(User user)
        {
            switch (user.GetStatus())
            {
                case UserStatus.Deleted:
                case UserStatus.Hidden:
                    AddError(ErrorStatus.NotFound);
                    return false;
                case UserStatus.Unconfirmed:
                    AddError(ErrorStatus.AccessDenied); return false;
                case UserStatus.Banned:
                    AddError(ErrorStatus.AccessDenied); return false;
                case UserStatus.TimeLock:
                    AddError(ErrorStatus.AccessDenied, user.BanExpireDate); return false;
                default: return true;
            }
        }
        public bool IsExist<T>(T value)
        {
            if (value != null) return true;
            AddError(ErrorStatus.NotFound);
            return false;
        }
        public bool IsNotEqual<T>(T value, T newValue)
        {
            if (!value.Equals(newValue)) return true;
            AddError(ErrorStatus.AlreadyOccupied);
            return false;
        }
    }
}
