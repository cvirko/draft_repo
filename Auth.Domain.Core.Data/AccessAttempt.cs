using Auth.Domain.Core.Common.Extensions;

namespace Auth.Domain.Core.Data
{
    public abstract class AccessAttempt: TEntity
    {
        public short Attempts { get; set; } = 5;
        public DateTime CreationDate { get; set; } = DateTimeExtension.Get();
    }
}
