using Auth.Infrastructure.Logic.Validation.ValidationRules;


namespace Auth.Infrastructure.Logic.Validation.UnitOfWork
{
    internal class UnitOfWorkValidationRule(IValidationMessageService message, RegexService regex) : IUnitOfWorkValidationRule
    {
        private string fieldName = AppConsts.GLOBALERROR;
        private readonly IValidationMessageService _message = message;
        private readonly RegexService _regex = regex;
        private ValidationError[] errors;

        private IEmailValidationRule _emailRule = default;
        private INameValidationRule _nameRule = default;
        private IPasswordValidationRule _passRule = default;
        private IUserValidationRule _userRule = default;
        private ITokenValidationRule _tokenRule = default;
        private IFileValidationRule _fileRule = default;
        private IMessageValidationRule _messageRule = default;

        public void SetFieldName(string field) => fieldName = field.ToLowerFirstChar();
        public IEmailValidationRule Email() => _emailRule ?? new EmailValidationRule(_regex, AddError);
        public INameValidationRule Name() => _nameRule ?? new NameValidationRule(_regex, AddError);
        public IPasswordValidationRule Password() => _passRule ?? new PasswordValidationRule(_regex, AddError);
        public IUserValidationRule User() => _userRule ?? new UserValidationRule(_regex, AddError);
        public ITokenValidationRule Token() => _tokenRule ?? new TokenValidationRule(_regex, AddError);
        public IFileValidationRule File() => _fileRule ?? new FileValidationRule(AddError);
        public IMessageValidationRule Message() => _messageRule ?? new MessageValidationRule(_regex, AddError);
        public void AddError(ErrorStatus status, params object[] values)
        {
            if (IsValid())
                errors = new ValidationError[AppConsts.ERRORS_MAX_LENGTH];
            for (int i = 0; i < errors.Length; i++)
            {
                if (errors[i] != null) continue;
                errors[i] = _message.Get(status, fieldName, values);
                break;
            }
        }

        public bool IsValid() => errors == null;
        public ValidationError[] GetErrors()
        {
            if (IsValid()) return default;
            var count = errors.Count(p => p != null);
            if (count == AppConsts.ERRORS_MAX_LENGTH)
                return errors;
            var newArray = new ValidationError[count];
            for (var i = 0; i < count; i++)
                newArray[i] = errors[i];
            return newArray;
        }
    }
}
