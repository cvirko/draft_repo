using Auth.Domain.Core.Logic.Models.Tokens;

namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class SignInSocialCommand : Command
    {
        public SocialData Info { get; set; }

    }
}
