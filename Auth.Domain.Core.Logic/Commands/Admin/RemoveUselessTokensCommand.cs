namespace Auth.Domain.Core.Logic.Commands.Admin
{
    public class RemoveUselessTokensCommand : Command
    {
        public CancellationToken Token { get; set; }
    }
}
