using System.Net;

namespace Auth.Domain.Core.Common.Exceptions
{
    public class ForbiddenException : ResponseException
    {
        public ForbiddenException(string at, string message, 
            IEnumerable<string> textErrors = null) 
            : base(at, message, HttpStatusCode.Forbidden, textErrors)
        {
        }
        public ForbiddenException(string at, string messageFormat, params ReadOnlySpan<object> text) : base(at, string.Format(messageFormat, text), HttpStatusCode.Forbidden)
        {
        }
        public ForbiddenException(string at, IEnumerable<string> textErrors = null) 
            : base(at, "", HttpStatusCode.Forbidden, textErrors)
        {
        }
    }
    public class ForbiddenException<T> : ResponseException<T>
    {
        public ForbiddenException(string at, string message, IEnumerable<T> validationErrors = null,
            IEnumerable<string> textErrors = null) : base(at, message, HttpStatusCode.Forbidden, validationErrors, textErrors)
        {
        }
        public ForbiddenException(string at, IEnumerable<T> validationErrors = null,
            IEnumerable<string> textErrors = null) : base(at, "", HttpStatusCode.Forbidden, validationErrors, textErrors)
        {
        }
    }
}
