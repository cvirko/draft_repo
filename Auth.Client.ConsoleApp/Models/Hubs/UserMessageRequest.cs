namespace Auth.Client.ConsoleApp.Models.Hubs
{
    internal class UserMessageRequest
    {
        public string SenderId { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
    }
}
