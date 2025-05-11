using Auth.Domain.Core.Common.Extensions;

namespace Auth.Domain.Core.Data
{
    public abstract class AccessAttempt(ushort attempts = 5) : TEntity
    {
        public ushort Attempts { get; set; } = attempts;
        public DateTime DateAt { get; set; } = DateTimeExtension.Get();
    }
}
