namespace Auth.Domain.Core.Common.Enums
{
    [Flags]
    public enum OrderStatus
    {
        None = -1,
        Canceled = 0,
        Paid = 1,
        Pending = 2,
        Expired =  3,
        Fail = 4,
        Collecting
    }
}
