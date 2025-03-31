namespace Auth.Client.ConsoleApp.Models.Enums
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
