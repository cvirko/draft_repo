using Auth.Domain.Core.Common.Enums;

namespace Auth.Domain.Core.Common.Exceptions
{
    public class ErrorText
    {
        public string Text { get; set; }
        public ErrorStatus StatusName { get; set; }
    }
}
