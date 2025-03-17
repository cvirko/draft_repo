namespace Auth.Domain.Core.Logic.Models.Hub
{
    public class UserMessage
    {
        public Guid UserId { get; set; }
        public string Id => UserId.ToString();
        public string UserName { get; set; }
        public string GroupName { get; set; }
        public string Text { get; set; }
    }
}
