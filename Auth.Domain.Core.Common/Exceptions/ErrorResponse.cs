namespace Auth.Domain.Core.Common.Exceptions
{
    public class ErrorResponse
    {
        public IEnumerable<ValidationError> ValidationErrors { get; set; }

        public string Message { get; set; }

        public string Detail { get; set; }
    }
}
