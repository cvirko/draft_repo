using Library.CommandMediator.Models;
using System.Net;

namespace Library.CommandMediator.Exceptions
{
    public class ValidationException<TError, EStatus> : ResponseException<TError, EStatus>
        where TError : BaseValidationError<EStatus>
        where EStatus : Enum
    {
        public ValidationException(string at, string message, IEnumerable<TError> validationErrors = null,
            IEnumerable<string> textErrors = null)
            : base(at, message, HttpStatusCode.Forbidden, validationErrors, textErrors)
        {
        }
        public ValidationException(string at, IEnumerable<TError> validationErrors = null,
            IEnumerable<string> textErrors = null)
            : base(at, "", HttpStatusCode.Forbidden, validationErrors, textErrors)
        {
        }
        public ValidationException(string at, string messageFormat, params ReadOnlySpan<object> text)
            : base(at, string.Format(messageFormat, text), HttpStatusCode.Forbidden)
        {
        }
    }
}
