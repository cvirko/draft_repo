using Library.CommandMediator;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auth.Infrastructure.Logic.Validation
{
    public static class IoCValidationServices
    {
        public static void RegistrationValidationService(this IServiceCollection services)
        {
            var classesIoc = Assembly.GetExecutingAssembly().GetIocTypes();
            services.AddValidators(classesIoc);
            services.AddValidationRule<IValidationRuleService, ValidationRuleService, ValidationError, ErrorStatus>();
            services.AddScoped<IValidationRuleService, ValidationRuleService>();
        }
    }
}
