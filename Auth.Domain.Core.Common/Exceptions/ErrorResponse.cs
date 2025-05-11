namespace Auth.Domain.Core.Common.Exceptions
{
    public class ErrorResponse<T> : ErrorResponse
    {
        public IEnumerable<T> ValidationErrors { get; set; }
    }
    public class ErrorResponse
    {
        public string Message { get; set; }

        public string Detail { get; set; }
    }
}
