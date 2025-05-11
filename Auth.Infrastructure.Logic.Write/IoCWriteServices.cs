using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;
using Auth.Infrastructure.Logic.Write.Workers;
using Library.CommandMediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Auth.Infrastructure.Logic.Write
{
    public static class IoCWriteServices
    {
        public static void RegistrationWriteService(this IServiceCollection services)
        {
            var classesIoc = Assembly.GetExecutingAssembly().GetIocTypes();
            services.AddHandlers(classesIoc);
            services.AddCommandMediator<ValidationError, ErrorStatus>();
        }
        public static void AddWorkers(this IServiceCollection services)
        {
            services.AddHostedService<InitialWorker>();
            services.AddHostedService<PaymentWorker>();
        }
        public static void UseValidationMessages(this IHost host, FilesOptions options)
        {
            using (var scope = host.Services.CreateScope())
            {
                var builder = scope.ServiceProvider.GetRequiredService<IFileBuilder>();
                var date = builder.GetFromJson
                    <IEnumerable<ErrorMessage>>
                    (options.ErrorsTextPath)
                    .ToDictionary(p => p.StatusName, p => p.Text);
                host.UseCommandMediator(date);
            }
        }
        private record ErrorMessage(ErrorStatus StatusName, string Text);
    }
}
