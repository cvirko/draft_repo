using Microsoft.AspNetCore.Http;

namespace Auth.Domain.Core.Common.Extensions
{
    public static class RequestExtension
    {
        private const string SEPERATOR = ";";
        public static string GetUserInfo(this HttpRequest request) => request.GetClientInfo() + request.GetIpAddress();
        public static string GetClientInfo(this HttpRequest request)
        {
            string info = null;
            if (request.Headers.TryGetValue("sec-ch-ua-platform", out var platform))
            {
                info += platform[0].Trim('\"') + SEPERATOR;
            }
            if (request.Headers.TryGetValue("sec-ch-ua", out var browser))
            {
                var a = browser.SelectMany(p =>
                    p.Split(", ").Select(p => p.Split(";")?[0].Trim('\"')))
                    .ToArray();
                info += string.Join(SEPERATOR, a);
            }
            if (request.Headers.TryGetValue("sec-ch-ua-mobile", out var isMobile))
            {
                info += isMobile;
            }
            
            if (info is null && 
                request.Headers.TryGetValue("User-Agent", out var userAgent))
            {
                info += userAgent;
            }
            
            return info;
        }
        public static string GetIpAddress(this HttpRequest request)

        {
            var ipAddress = request?.Headers?["X-Real-IP"].ToString();

            if (!string.IsNullOrEmpty(ipAddress))
            {
                return ipAddress;
            }
            ipAddress = request?.Headers?["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(ipAddress))
            {
                var parts = ipAddress.Split(',');
                if (parts.Length > 0)
                {
                    ipAddress = parts[0];
                }
                return ipAddress;
            }
            ipAddress = request?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            if (!string.IsNullOrEmpty(ipAddress))
            {
                return ipAddress;
            }

            return string.Empty;

        }
        public static string GetDomain(this HttpRequest request) => string.Format("{0}://{1}",request.Scheme,request.Host);
    }
}
