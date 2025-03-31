namespace Auth.Client.ConsoleApp.Tools
{
    internal class RabbitMQOptions
    {
        public Host[] Hosts { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ProvidedName { get; set; }
        public ushort ChannelsMax { get; set; }
    }
    public class Host
    {
        public string Name { get; set; }
        public int Port { get; set; }
    }
}
