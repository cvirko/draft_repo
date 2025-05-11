namespace Library.CommandMediator.Models
{
    public record BaseValidationError
    {
        public BaseValidationError() { }
        public BaseValidationError(
            string field,
            string title = default,
            string description = default)
        {
            Field = field;
            Title = title;
            Description = description;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Field { get; set; }
    }
    public record BaseValidationError<EStatus> : BaseValidationError where EStatus : Enum
    {
        public BaseValidationError() { }
        public BaseValidationError(
            string field, 
            string title = default, 
            EStatus type = default, 
            string description = default) : base(field, title, description)
        {
            TitleEnum = type;
        }
        public EStatus TitleEnum { get; set; }
    }
}
