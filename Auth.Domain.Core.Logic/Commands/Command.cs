namespace Auth.Domain.Core.Logic.Commands
{
    public abstract class Command
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public ulong LoginId { get; set; } = 0;
    }
}
