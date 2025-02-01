namespace Auth.Domain.Core.Logic.Models.Tokens
{
    public class TokenData
    {
        public TokenType TokenType { get; set; }
        public DateTime Expires { get; set; }
        public string Token { get; set; }
    }
}
