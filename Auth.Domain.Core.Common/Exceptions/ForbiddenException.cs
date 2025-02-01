using System.Net;

namespace Auth.Domain.Core.Common.Exceptions
{
    public class ForbiddenException : ResponseException
    {
        public ForbiddenException(string message, IEnumerable<ValidationError> validationErrors = null,
            IEnumerable<string> textErrors = null) : base(message, HttpStatusCode.Forbidden, validationErrors, textErrors)
        {
        }
        public ForbiddenException(IEnumerable<ValidationError> validationErrors = null,
            IEnumerable<string> textErrors = null) : base("", HttpStatusCode.Forbidden, validationErrors, textErrors)
        {
        }
    }
}
