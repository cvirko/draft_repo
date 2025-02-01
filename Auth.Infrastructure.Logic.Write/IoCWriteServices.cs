using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auth.Infrastructure.Logic.Write
{
    public static class IoCWriteServices
    {
        public static void RegistrationWriteService(this IServiceCollection services)
        {
            var classesIoc = Assembly.GetExecutingAssembly().GetIocTypes();
            services.AddScoped<ICommandHandler<Command>>(classesIoc);
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<ICommandHandlerFactory, CommandHandlerFactory>();
        }
        private static void AddScoped<T>(this IServiceCollection services, Type[] types)
        {
            var classes = types.GetClasses<T>();
            for (var i = 0; i < classes.Length; i++)
            {
                services.AddScoped(classes[i].GetInterfaces()[0], classes[i]);
            }
        }
    }
}
