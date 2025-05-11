using System.Net;

namespace Auth.Domain.Core.Common.Exceptions
{
    public class ResponseException<T> : ResponseException
    {
        public IEnumerable<T> Errors { get; }
        public ResponseException(
            string at,
            string message,
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError,
            IEnumerable<T> errors = null,
            IEnumerable<string> textErrors = null) : base(at, message, httpStatusCode, textErrors)
        {
            Errors = errors;
        }
    }
    public class ResponseException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public IEnumerable<string> TextErrors { get; }
        public string At { get; }
        public ResponseException(
            string at, string message,
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError,
            IEnumerable<string> textErrors = null) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            TextErrors = textErrors;
            At = at;
        }
    }
}
