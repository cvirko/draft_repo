namespace Auth.Domain.Core.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static bool IsNotExpire(this DateTime date) => Get() < date;
        public static TimeSpan RemainingTime(this DateTime date) => date - Get();
        public static DateTime Get() => DateTime.UtcNow;
        public static DateTime WithMinutes(float minutes) => Get().AddMinutes(minutes);
        public static DateTime WithDays(float days) => Get().AddDays(days);
    }
}
