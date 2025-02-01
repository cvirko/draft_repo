using Auth.Domain.Interface.Logic.Read.ModelBuilder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auth.Infrastructure.Logic.Read
{
    public static class IoCReadModelServices
    {
        public static void RegistrationReadModelService(this IServiceCollection services)
        {
            var classesIoc = Assembly.GetExecutingAssembly().GetIocTypes();
            services.AddScoped<IMapper>(classesIoc);
            services.AddScoped<IBuilder>(classesIoc);
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
