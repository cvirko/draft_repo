using Library.CommandMediator.Consts;
using Library.CommandMediator.Exceptions;
using Library.CommandMediator.Extinsions;
using Library.CommandMediator.Interfaces;
using Library.CommandMediator.Models;

namespace Library.CommandMediator.Services
{
    public abstract class BaseValidationRuleService<TError, EStatus>(IValidationMessageService<TError, EStatus> message, IRegexService regex) 
        : IBaseValidationRuleService<TError, EStatus>
        where TError : BaseValidationError<EStatus>
        where EStatus : Enum
    {
        private string fieldName = ExceptionConst.GLOBALERROR;
        private readonly IValidationMessageService<TError, EStatus> _message = message;
        protected readonly IRegexService _regex = regex;
        private TError[] errors;

        public void SetFieldName(string field) => fieldName = field.ToLowerFirstChar();
        public void AddError(EStatus status, params object[] values)
        {
            if (IsValid)
                errors = new TError[ExceptionConst.ERRORS_MAX_LENGTH];
            for (int i = 0; i < errors.Length; i++)
            {
                if (errors[i] != null) continue;
                errors[i] = _message.Get(status, fieldName, values);
                break;
            }
        }

        public bool IsValid => errors == null;
        public TError[] GetErrors()
        {
            if (IsValid) return default;
            var count = errors.Count(p => p != null);
            if (count == ExceptionConst.ERRORS_MAX_LENGTH)
                return errors;
            var newArray = new TError[count];
            for (var i = 0; i < count; i++)
                newArray[i] = errors[i];
            return newArray;
        }
        public void Throw(Type type, string message = ExceptionConst.MESSAGE)
            => Throw(type.Name, message);
        public void Throw(string at, string message = ExceptionConst.MESSAGE)
            => throw new ValidationException<TError, EStatus>(at, message, GetErrors());
    }
}
