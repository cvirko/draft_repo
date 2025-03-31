using System.Net;

namespace Auth.Client.ConsoleApp.Models.Exceptions
{
    public class ResponseException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public IEnumerable<ValidationError> Errors { get; }
        public IEnumerable<string> TextErrors { get; }
        public ResponseException(
            string message,
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError,
            IEnumerable<ValidationError> errors = null,
            IEnumerable<string> textErrors = null) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            Errors = errors;
            TextErrors = textErrors;
        }
    }
}
