namespace Auth.Domain.Core.Common.Tools.Configurations
{
    public class FailedAccessOptions: Options
    {
        public float TimeLockInMinutes { get; set; }
        public byte FailedAccessAttemptsMaxCount { get; set; }
    }
}
