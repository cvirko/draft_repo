using Auth.Client.ConsoleApp.Models.Enums;

namespace Auth.Client.ConsoleApp.Models.Tokens
{
    public class TokenData
    {
        public TokenType TokenType { get; set; }
        public DateTime Expires { get; set; }
        public string Token { get; set; }
    }
}
