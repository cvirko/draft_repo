using Auth.Domain.Core.Data.DBEntity.Account;

namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class EmailValidationRule(IRegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRule<string>(regex, action), IEmailValidationRule
    {
        public override bool IsLengthFormatValid(string value)
        {
            if (IsLengthInvalid(value, new Range(7, 64)))
                return false;

            if (_regex.IsEmailInvalidFormat(value))
            {
                AddError(ErrorStatus.Format);
                return false;
            }
            return true;
        }
        public async Task<bool> IsNotOccupiedAsync(string value, Func<string, Task<bool>> queryAsync)
        {
            if (!await queryAsync(value))
                return true;
            AddError(ErrorStatus.AlreadyOccupied);
            return false;
        }
        public bool IsExist(UserLogin value)
        {
            if (value?.Email != null)
                return true;
            AddError(ErrorStatus.NotFound);
            return false;
        }

        public async Task<bool> IsExistAsync(string value, Func<string, Task<bool>> queryAsync)
        {
            if (await queryAsync(value))
                return true;
            AddError(ErrorStatus.NotFound);
            return false;
        }

        public bool IsDelayGone(string email, DateTime? lastMailDate, double delayBetweenMessagesInMinutes = 1)
        {
            if (lastMailDate == null)
                return true;
            var nextMsgAvailable = lastMailDate.Value.AddMinutes(delayBetweenMessagesInMinutes);
            if (nextMsgAvailable > DateTimeExtension.Get())
            {
                AddError(ErrorStatus.AccessDenied, nextMsgAvailable);
                return false;
            }

            return true;

        }
    }
}
