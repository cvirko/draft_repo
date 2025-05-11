namespace Auth.Domain.Core.Common.Extensions
{
    public static class AvatarExtension
    {
        public static string ToFileName(this Guid userId, string type = "png") => string.Format("{0}.{1}", userId, type);
    }
}
