using Library.CommandMediator.Interfaces;
using System.Text.RegularExpressions;

namespace Library.CommandMediator.Services
{
    public class RegexService : IRegexService
    {
        private bool IsInvalid(string input, string pattern) => !Regex.Match(input, pattern).Success;
        public bool IsFieldEmpty(string value) => string.IsNullOrWhiteSpace(value);

        public bool IsFieldInvalidLength(string str, Range range)
        {
            if (range.Start.Value > 0 && IsFieldEmpty(str))
                return true;
            return str.Length > range.End.Value || str.Length < range.Start.Value;
        }
        public bool IsNoRequiredUppercaseLatin(string str)
        {
            if (IsFieldEmpty(str))
                return true;
            return IsInvalid(str, @"^([A-Z])$");
        }
        public bool IsNoRequiredLowercaseLatin(string str)
        {
            if (IsFieldEmpty(str))
                return true;
            return IsInvalid(str, @"^([a-z])$");
        }
        public bool IsNoRequiredNumbers(string str)
        {
            if (IsFieldEmpty(str))
                return true;
            return IsInvalid(str, @"^([0-9])$");
        }
        public bool IsPasswordInvalidFormat(string str)
        {
            if (IsFieldEmpty(str))
                return true;
            return IsInvalid(str, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^_&.*-+:;()/[]|=]).{1,}$");
        }
        public bool IsEmailInvalidFormat(string str)
        {
            if (IsFieldEmpty(str))
                return true;
            return IsInvalid(str, "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
        }
        public bool IsNotOnlyNumbersLowercaseUppercaseLatin(string str)
        {
            if (IsFieldEmpty(str))
                return false;
            return IsInvalid(str, @"^([A-Za-z0-9]*)$");
        }
        public bool IsNotOnlyNumbers(string str)
        {
            if (IsFieldEmpty(str))
                return false;
            return IsInvalid(str, @"^([0-9]*)$");
        }
    }
}
