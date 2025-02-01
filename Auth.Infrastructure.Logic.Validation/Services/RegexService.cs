using System.Text.RegularExpressions;

namespace Auth.Infrastructure.Logic.Validation.Services
{
    internal class RegexService
    {
        internal Dictionary<string, Range> Length = new()
        {
            {nameof(UnitOfWorkValidationRule.Email),new Range(7, 64) },
            {nameof(UnitOfWorkValidationRule.Name),new Range(2, 32) },
            {nameof(UnitOfWorkValidationRule.Password),new Range(8, 64) },
            {nameof(UnitOfWorkValidationRule.User),new Range(8, 50) },
            {nameof(UnitOfWorkValidationRule.Token),new Range(6, 50) },
            {AppConsts.GLOBALERROR, new Range(1,1) }
        };
        private bool IsInvalid(string input, string pattern) => !Regex.Match(input, pattern).Success;
        internal bool IsFieldEmpty(string value) => string.IsNullOrWhiteSpace(value);

        internal bool IsFieldInvalidLength(string str, Range range)
        {
            if (range.Start.Value > 0 && IsFieldEmpty(str))
                return true;
            return str.Length > range.End.Value || str.Length < range.Start.Value;
        }
        internal bool IsNoRequiredUppercaseLatin(string str)
        {
            if (IsFieldEmpty(str))
                return true;
            return IsInvalid(str, @"^([A-Z])$");
        }
        internal bool IsNoRequiredLowercaseLatin(string str)
        {
            if (IsFieldEmpty(str))
                return true;
            return IsInvalid(str, @"^([a-z])$");
        }
        internal bool IsNoRequiredNumbers(string str)
        {
            if (IsFieldEmpty(str))
                return true;
            return IsInvalid(str, @"^([0-9])$");
        }
        internal bool IsPasswordInvalidFormat(string str)
        {
            if (IsFieldEmpty(str))
                return true;
            return IsInvalid(str, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^_&.*-+:;()/[]|=]).{1,}$");
        }
        internal bool IsEmailInvalidFormat(string str)
        {
            if (IsFieldEmpty(str))
                return true;
            return IsInvalid(str, "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
        }
        internal bool IsNotOnlyNumbersLowercaseUppercaseLatin(string str)
        {
            if (IsFieldEmpty(str))
                return false;
            return IsInvalid(str, @"^([A-Za-z0-9]*)$");
        }
        internal bool IsNotOnlyNumbers(string str)
        {
            if (IsFieldEmpty(str))
                return false;
            return IsInvalid(str, @"^([0-9]*)$");
        }
    }
}
