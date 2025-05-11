using Library.CommandMediator.Consts;
using Library.CommandMediator.Interfaces;
using Library.CommandMediator.Models;

namespace Library.CommandMediator.Services
{
    internal class ValidationMessageService<TError, EStatus> 
        (IValidationMessageData<EStatus> errors)
        : IValidationMessageService<TError, EStatus>
        where TError : BaseValidationError<EStatus>, new()
        where EStatus : Enum
    {
        private IValidationMessageData<EStatus> _errors = errors;

        public TError Get(EStatus status,
            string field = ExceptionConst.GLOBALERROR, params object[] values)
        {
            if (_errors == null || !_errors.TryGetValue(status, out string message))
                return Create(field, default, status);

            if (values.Length == 0)
                return Create(field, message, status);

            return Create(field, Replace(message, values),
                        status, Join(values));
        }
        private TError Create(string field, string message, EStatus status, string paremeters = null)
        {
            return new TError
            {
                Field = field,
                Title = message,
                TitleEnum = status,
                Description = paremeters
            };
        }
        private string Join(params object[] value)
        {
            return string.Join(ExceptionConst.SEPARATOR, value);
        }
        private string Replace(string message, params object[] replaces)
        {
            return string.Format(message, replaces);
        }
    }
}
