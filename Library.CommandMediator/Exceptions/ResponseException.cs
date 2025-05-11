using Library.CommandMediator.Models;
using System.Net;

namespace Library.CommandMediator.Exceptions
{
    public class ResponseException<TError, EStatus> : Exception
        where TError : BaseValidationError<EStatus>
        where EStatus : Enum
    {
        public HttpStatusCode HttpStatusCode { get; }
        public IEnumerable<TError> Errors { get; }
        public IEnumerable<string> TextErrors { get; }
        public string At { get; }
        public ResponseException(
            string at,
            string message,
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError,
            IEnumerable<TError> errors = null,
            IEnumerable<string> textErrors = null) : base(message)
        {
            At = at;
            HttpStatusCode = httpStatusCode;
            Errors = errors;
            TextErrors = textErrors;
        }
    }
}
