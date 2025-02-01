using Microsoft.AspNetCore.Http;

namespace Auth.Domain.Core.Common.Extensions
{
    public static class RequestExtension
    {
        public static string GetDomain(this HttpRequest request) => string.Format("{0}://{1}",request.Scheme,request.Host);
    }
}
