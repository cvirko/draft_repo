namespace Auth.Domain.Core.Logic.Models.Hub
{
    public class UserHubMessageRequest
    {
        public UserHubMessageRequest()
        {
            
        }
        public UserHubMessageRequest(UserHubMessage message)
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
