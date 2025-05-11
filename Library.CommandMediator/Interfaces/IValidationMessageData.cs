namespace Library.CommandMediator.Interfaces
{
    internal interface IValidationMessageData<EStatus>
        where EStatus : Enum
    {
        public IReadOnlyDictionary<EStatus, string> Messages { get; set; }
        public bool TryGetValue(EStatus status, out string value);
    }
}
