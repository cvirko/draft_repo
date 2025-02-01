namespace Auth.Domain.Core.Common.Enums
{
    public enum TokenType: byte
    {
        Eternal,
        Refresh,
        Access,
        ConfirmMail,
        ConfirmPassword,
        Reset,
    }
}
