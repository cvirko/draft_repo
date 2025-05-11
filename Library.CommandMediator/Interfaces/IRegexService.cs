namespace Library.CommandMediator.Interfaces
{
    public interface IRegexService
    {
        public bool IsFieldEmpty(string value) => string.IsNullOrWhiteSpace(value);

        public bool IsFieldInvalidLength(string str, Range range);
        public bool IsNoRequiredUppercaseLatin(string str);
        public bool IsNoRequiredLowercaseLatin(string str);
        public bool IsNoRequiredNumbers(string str);
        public bool IsPasswordInvalidFormat(string str);
        public bool IsEmailInvalidFormat(string str);
        public bool IsNotOnlyNumbersLowercaseUppercaseLatin(string str);
        public bool IsNotOnlyNumbers(string str);
    }
}
