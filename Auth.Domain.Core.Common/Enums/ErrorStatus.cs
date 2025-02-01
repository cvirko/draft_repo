namespace Auth.Domain.Core.Common.Enums
{
    [Flags]
    public enum ErrorStatus : ushort
    {
        HasError,
        NotFound,
        AccessDenied,
        Invalid,
        Length,
        Format,
        AlreadyOccupied,
        AlreadyConfirmed,
        NoAttempts,
        ConfirmMail

    }
}
