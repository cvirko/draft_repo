using Auth.Client.ConsoleApp.Models.Enums;

namespace Auth.Client.ConsoleApp.Models.Exceptions
{
    public class ValidationError
    {
        public ValidationError(string field, string title = null, ErrorStatus type = 0, string description = null)
        {
            Field = field;
            Title = title;
            Description = description;
            TitleEnum = type;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Field { get; set; }
        public ErrorStatus TitleEnum { get; set; }
    }
}
