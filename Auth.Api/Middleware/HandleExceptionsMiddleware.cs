using Auth.Domain.Core.Common.Exceptions;
using System.Text;
using System.Text.Json;


namespace Auth.Api.Middleware
{
    public class HandleExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ILogger<HandleExceptionsMiddleware> _logger;
        public HandleExceptionsMiddleware(RequestDelegate next, IWebHostEnvironment webHostEnvironment, ILogger<HandleExceptionsMiddleware> logger)
        {
            this._next = next;
            this._webHostEnvironment = webHostEnvironment;

            this._logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(
           HttpContext context,
           Exception exception)
        {
            var errorResponse = new ErrorResponse();
            Exception logEx = null;
            if (_webHostEnvironment.IsDevelopment())
            {
                errorResponse.Message = exception.Message;
                errorResponse.Detail = TrimException(exception).ToString();
            }
            if (exception is ResponseException responseException)
            {
                context.Response.StatusCode = (int)responseException.HttpStatusCode;
                errorResponse.ValidationErrors = responseException.Errors;
                errorResponse.Message = responseException.Message;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                errorResponse.Message = "Uncaught application error";
                logEx = exception;
            }

            context.Response.ContentType = MIMEType.Json;
            var stringEx = JsonSerializer.Serialize(errorResponse);
            var dataToWriteToBody = stringEx.ToByteArray();

            _logger.LogError(logEx, stringEx);
            await context.Response.Body.WriteAsync(dataToWriteToBody, 0, stringEx.Length);
        }

        private static object TrimException(Exception exception)
        {
            if (exception == null) return default;

            return new
            {
                exception.Message,
                StackTrace = TrimStackTrace(exception.StackTrace),
                InnerException = TrimException(exception.InnerException),
                InnerExceptions = exception is AggregateException ae
                    ? ae.InnerExceptions.Select(TrimException)
                    : Array.Empty<string>()
            };
        }
        private static string TrimStackTrace(string trace)
        {
            if (string.IsNullOrEmpty(trace)) return string.Empty;
            var strBuilder = new StringBuilder();
            const string stratText = "at Auth.Api";
            const string endText = "\r\n";
            var endStrIndex = 0;
            while (true)
            {
                var startStrIndex = trace.IndexOf(stratText, endStrIndex);
                if (startStrIndex == -1) break;
                endStrIndex = trace.IndexOf(endText, startStrIndex);
                if (endStrIndex == -1)
                    endStrIndex = trace.Length - endText.Length;
                strBuilder.Append(trace, startStrIndex, endStrIndex - startStrIndex + endText.Length);
            }
            return strBuilder.ToString();
        }
    }
}
