namespace Auth.Domain.Core.Common.Tools.Configurations
{
    public class CacheOptions: Options
    {
        public float AbsoluteTimeStorageInMinutes { get; set; }
        public float SlidingTimeStorageInMinutes { get; set; }
    }
}
