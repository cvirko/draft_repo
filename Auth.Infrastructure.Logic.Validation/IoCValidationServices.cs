using Auth.Domain.Core.Logic.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auth.Infrastructure.Logic.Validation
{
    public static class IoCValidationServices
    {
        public static void RegistrationValidationService(this IServiceCollection services)
        {
            var classesIoc = Assembly.GetExecutingAssembly().GetIocTypes();
            services.AddScoped<IValidator<Command>>(classesIoc);
            services.AddSingleton<RegexService>();
            services.AddScoped<IValidationMessageService, ValidationMessageService>();
            services.AddScoped<IUnitOfWorkValidationRule, UnitOfWorkValidationRule>();
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
