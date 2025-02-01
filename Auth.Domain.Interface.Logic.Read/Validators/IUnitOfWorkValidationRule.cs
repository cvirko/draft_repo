using Auth.Domain.Core.Common.Exceptions;
using Auth.Domain.Interface.Logic.Read.Validators.Rules;

namespace Auth.Domain.Interface.Logic.Read.Validators
{
    public interface IUnitOfWorkValidationRule
    {
        void SetFieldName(string field);
        IEmailValidationRule Email();
        INameValidationRule Name();
        IPasswordValidationRule Password();
        IUserValidationRule User();
        ITokenValidationRule Token();
        IFileValidationRule File();
        void AddError(ErrorStatus status,
            params object[] values);
        bool IsValid();
        ValidationError[] GetErrors();
    }
}
