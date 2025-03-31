using Microsoft.Extensions.Configuration;

namespace Auth.Client.ConsoleApp.Services.Configurations
{
    internal class ConfigurationService
    {
        private IConfigurationRoot configuration;
        public ConfigurationService()
        {
            AddSettings();
        }
        private void AddSettings()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();

            builder
               .AddJsonFile("appsettings.Development.json", optional: false);

            configuration = builder.Build();
        }
        public T Get<T>(string name)
        {
            return configuration.GetSection(name).Get<T>();
        }
    }
}
