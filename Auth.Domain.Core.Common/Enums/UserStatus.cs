namespace Auth.Domain.Core.Common.Enums
{
    public enum UserStatus: byte
    {
        Active,
        Banned,
        TimeLock,
        Test,
        Deleted,
        Hidden,
        Unconfirmed
    }
}
