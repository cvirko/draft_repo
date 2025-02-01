namespace Auth.Domain.Core.Common.Tools.Configurations
{
    public class ConnectionOptions: Options
    {
        public string Database { get; set; }
        public string DatabaseRead { get; set; }
        public string Redis { get; set; }
    }
}
