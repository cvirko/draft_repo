namespace Auth.Domain.Core.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime Get() => DateTime.UtcNow;
        public static DateTime WithMinutes(float minutes) => Get().AddMinutes(minutes);
    }
}
