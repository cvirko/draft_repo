namespace Auth.Domain.Core.Logic.Models.Hub
{
    public class UserMessageRequest
    {
        public UserMessageRequest()
        {
            
        }
        public UserMessageRequest(UserMessage message)
        {
            SenderId = message.Id;
            Message = message.Text;
            Sender = message.UserName;
        }
        public string SenderId { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
    }
}
