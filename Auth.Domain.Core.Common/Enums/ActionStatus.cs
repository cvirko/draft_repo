namespace Auth.Domain.Core.Common.Enums
{
    [Flags]
    public enum ActionStatus
    {
        None = -1,
        Canceled = 0,
        Successed = 1,
        Pending = 2,
        Expired =  3,
        Updated = 4,
        Fail = 5
    }
}
