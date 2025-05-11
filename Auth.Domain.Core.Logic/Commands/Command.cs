using Library.CommandMediator.Models;

namespace Auth.Domain.Core.Logic.Commands
{
    public abstract class Command  : CommandBase
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public ulong LoginId { get; set; } = 0;
    }
    public abstract class Command<TResponse> : CommandBase<TResponse>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public ulong LoginId { get; set; } = 0;

    }
}
