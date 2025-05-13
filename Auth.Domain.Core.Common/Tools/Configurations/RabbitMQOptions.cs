namespace Auth.Domain.Core.Common.Tools.Configurations
{
    public class RabbitMQOptions : Options
    {
        public Host[] Hosts { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ProvidedName { get; set; }
        public ushort ChannelsMax { get; set; }
        public bool IsUseListener { get; set; }
    }
    public class Host
    {
        public string Name { get; set; }
        public int Port { get; set; }
    }
}
