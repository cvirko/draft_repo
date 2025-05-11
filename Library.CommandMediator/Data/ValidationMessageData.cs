using Library.CommandMediator.Interfaces;

namespace Library.CommandMediator.Data
{
    internal class ValidationMessageData<EStatus> : IValidationMessageData<EStatus>
        where EStatus : Enum
    {
        public IReadOnlyDictionary<EStatus, string> Messages { get; set; }
        public bool TryGetValue(EStatus status, out string value)
            => Messages.TryGetValue(status, out value);
    }
}
