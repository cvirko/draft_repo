using Microsoft.AspNetCore.Http;

namespace Auth.Domain.Core.Common.Extensions
{
    public static class ResponseExtension
    {
        public static void CacheStoreDays(this HttpResponse response, float dayes = 30)
        {
            var headers = response.GetTypedHeaders();
            headers.CacheControl = new()
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(dayes)
            };
            headers.Expires = new DateTimeOffset(DateTimeExtension.WithDays(dayes));
        }
    }
}
