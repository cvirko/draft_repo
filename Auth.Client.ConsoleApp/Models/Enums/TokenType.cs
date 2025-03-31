namespace Auth.Client.ConsoleApp.Models.Enums
{
    public enum TokenType : byte
    {
        Eternal,
        Refresh,
        Access,
        ConfirmMail,
        ConfirmPassword,
        Reset,
    }
}
