using Auth.Infrastructure.Logic.Validation.ValidationRules;
using Library.CommandMediator.Services;


namespace Auth.Infrastructure.Logic.Validation.Services
{
    internal class ValidationRuleService(IValidationMessageService<ValidationError,ErrorStatus> message, IRegexService regex) 
        : BaseValidationRuleService<ValidationError, ErrorStatus>(message, regex), IValidationRuleService
    {
        private IEmailValidationRule _emailRule = default;
        private INameValidationRule _nameRule = default;
        private IPasswordValidationRule _passRule = default;
        private IUserValidationRule _userRule = default;
        private ITokenValidationRule _tokenRule = default;
        private IFileValidationRule _fileRule = default;
        private IMessageValidationRule _messageRule = default;
        private ITransactionValidationRule _transactionRule = default;
        private IBalanceValidationRule _balanceRule = default;

        public IBalanceValidationRule Balance()
        {
            if (_balanceRule is null)
                _balanceRule = new BalanceValidationRule(_regex, AddError);
            return _balanceRule;
        }
        public IEmailValidationRule Email()
        {
            if (_emailRule is null)
                _emailRule = new EmailValidationRule(_regex, AddError);
            return _emailRule;
        }
        public INameValidationRule Name()
        {
            if (_nameRule is null)
                _nameRule = new NameValidationRule(_regex, AddError);
            return _nameRule;
        }
        public IPasswordValidationRule Password()
        {
            if (_passRule is null)
                _passRule = new PasswordValidationRule(_regex, AddError);
            return _passRule;
        }
        public IUserValidationRule User()
        {
            if (_userRule is null)
                _userRule = new UserValidationRule(_regex, AddError);
            return _userRule;
        }
        public ITokenValidationRule Token()
        {
            if (_tokenRule is null)
                _tokenRule = new TokenValidationRule(_regex, AddError);
            return _tokenRule;
        }
        public IFileValidationRule File()
        {
            if (_fileRule is null)
                _fileRule = new FileValidationRule(AddError);
            return _fileRule;
        }

        public IMessageValidationRule Message()
        {
            if (_messageRule is null)
                _messageRule = new MessageValidationRule(_regex, AddError);
            return _messageRule;
        }
        public ITransactionValidationRule Transaction()
        {
            if (_transactionRule is null)
                _transactionRule = new TransactionValidationRule(_regex, AddError);
            return _transactionRule;
        }
    }
}
