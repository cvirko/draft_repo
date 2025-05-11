using Auth.Domain.Core.Common.Extensions;

namespace Auth.Domain.Core.Logic.Models.Hub
{
    public class UserHubConnection
    {
        public UserHubConnection(string connectionId)
        {
            ConnectionId = connectionId;
            Groups = [];
        }
        public string ConnectionId { get; set; }
        public DateTime? DisconnectedDate { get; set; }
        public bool IsDisconected => DisconnectedDate.HasValue 
            ? ((DateTimeExtension.Get() - DisconnectedDate.Value).TotalSeconds > 30) 
            : false;
        public ICollection<string> Groups { get; set; }
    }
}
