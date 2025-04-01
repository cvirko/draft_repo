using Auth.Domain.Core.Common.Consts;

namespace Auth.Domain.Core.Common.Extensions
{
    public static class FileExtension
    {
        public static string GetContentType(string extension)
        {
            return extension.Split(".").Last().ToLowerInvariant() switch
            {
                "mp4" => MIMEType.Mp4,
                "webm" => MIMEType.Webm,
                "ogg" => MIMEType.Ogg,
                "mov" => MIMEType.Quicktime,

                "png" => MIMEType.Png,
                "jpeg" => MIMEType.Jpep,

                "json" => MIMEType.Json,
                _ => "application/octet-stream"
            };
        }
        public static string GetExtension(string contentType)
        {
            return contentType switch
            {
                MIMEType.Mp4 => "mp4",
                MIMEType.Webm => "webm",
                MIMEType.Ogg => "ogg",
                MIMEType.Quicktime => "mov",

                MIMEType.Png => "png",
                MIMEType.Jpep => "jpeg",

                MIMEType.Json => "json",
                _ => "application/octet-stream"
            };
        }
    }
}
