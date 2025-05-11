using Auth.Domain.Core.Common.Exceptions;
using Library.CommandMediator.Exceptions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Auth.Api.Middleware
{
    public class HandleExceptionsMiddleware(RequestDelegate next, IWebHostEnvironment webHostEnvironment, ILogger<HandleExceptionsMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly ILogger<HandleExceptionsMiddleware> _logger = logger;

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ResponseException<ValidationError> ex)
            {
                await ValidationExceptionAsync(httpContext, ex, ex.Errors, ex.HttpStatusCode, ex.At);
            }
            catch (ResponseException<ValidationError, ErrorStatus> ex)
            {
                await ValidationExceptionAsync(httpContext, ex, ex.Errors, ex.HttpStatusCode, ex.At);
            }
            catch (ResponseException ex)
            {
                await ValidationExceptionAsync(httpContext, ex, default, ex.HttpStatusCode, ex.At);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task ValidationExceptionAsync(HttpContext context, Exception exception, IEnumerable<ValidationError> errors, HttpStatusCode statusCode, string at = null)
        {
            string stringEx = "";
            if (errors?.Any() ?? false)
                stringEx = JsonSerializer.Serialize(
                    new ErrorResponse<ValidationError>()
                {
                    ValidationErrors = errors,
                    Message = exception.Message,
                    Detail = _webHostEnvironment.IsDevelopment() 
                        ? at : default
                });
            else
                stringEx = JsonSerializer.Serialize(
                    new ErrorResponse()
                {
                    Message = exception.Message,
                    Detail = _webHostEnvironment.IsDevelopment() 
                        ? at : default
                });

            _logger.LogError("{0}: {1}", at, stringEx);

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = MIMEType.Json;
            await context.Response.Body.WriteAsync(stringEx.ToByteArray(), 0, stringEx.Length);
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var stringEx = JsonSerializer.Serialize( 
                new ErrorResponse()
                {
                    Message = "Uncaught application error",
                    Detail = !_webHostEnvironment.IsDevelopment() ? 
                        default :
                        Convert(TrimException(exception, new StringBuilder()))
                });

            _logger.LogError(exception, stringEx);

            context.Response.ContentType = MIMEType.Json;
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.Body.WriteAsync(stringEx.ToByteArray(), 0, stringEx.Length);
        }
        private string Convert(StringBuilder builder)
        {
            builder.Replace(AppConsts.PHYSICAL_ROOT_PATH, AppConsts.APP_NAME);
            return builder.ToString();
        }
        private StringBuilder TrimException(Exception exception, StringBuilder builder)
        {
            if (exception == null) return builder;
            if (!string.IsNullOrEmpty(exception.Message)) 
                builder.AppendLine(exception.Message);
            TrimStackTrace(exception.StackTrace, builder);
            TrimException(exception.InnerException, builder);
            if (exception is AggregateException ae)
                ae.InnerExceptions.Select(e => TrimException(e, builder));
            return builder;
        }
        
        const string START_LINE = "   at";
        private readonly static string[] LinesSkip = [
            string.Format("   at {0}.Api.Controllers.Write.CommandController.", AppConsts.APP_NAME),
            "   at Microsoft.AspNetCore",
            "   at lambda_",
            "   at System.Dynamic.UpdateDelegates",
            "--- End of stack",
            string.Format("   at {0}.Api.Middleware", AppConsts.APP_NAME)
            ];
        private StringBuilder TrimStackTrace(string trace, StringBuilder builder)
        {
            if (string.IsNullOrEmpty(trace)) return builder;
            
            int index = START_LINE.Length;
            while (index < trace.Length - 1) 
            {
                int startLineIndex = index - START_LINE.Length;
                if (!trace.TryIndexOf(START_LINE, index, out int endLineIndex))
                    endLineIndex = trace.Length - 1;

                var str = trace.Substring(startLineIndex, endLineIndex - startLineIndex);
                if (!trace.StartAny(LinesSkip, startLineIndex))
                    builder.Append(trace, startLineIndex, endLineIndex - startLineIndex);
                index = endLineIndex + START_LINE.Length;
            }
            return builder;
        }
    }
}