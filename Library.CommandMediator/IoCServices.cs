using Library.CommandMediator.Data;
using Library.CommandMediator.Extinsions;
using Library.CommandMediator.Interfaces;
using Library.CommandMediator.Models;
using Library.CommandMediator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Library.CommandMediator
{
    public static class IoCServices
    {
        public static IServiceCollection AddCommandMediator<TError, EStatus>(this IServiceCollection services)
            where TError : BaseValidationError<EStatus>, new()
            where EStatus : Enum
        {
            services.AddSingleton<IRegexService, RegexService>();
            services.AddSingleton<IValidationMessageData<EStatus>, ValidationMessageData<EStatus>>();
            services.AddScoped<IValidationMessageService<TError, EStatus>, ValidationMessageService<TError, EStatus>>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher<TError, EStatus>>();
            return services;
        }
        public static IServiceCollection AddValidationRule<TService, TImplementation, TError, EStatus>(this IServiceCollection services)
            where TService : class, IBaseValidationRuleService<TError, EStatus>
            where TImplementation : BaseValidationRuleService<TError, EStatus>, TService
            where TError : BaseValidationError<EStatus>
            where EStatus : Enum
        {
            services.AddScoped<TService, TImplementation>();
            return services;
        }
        public static IServiceCollection AddValidators(this IServiceCollection services, Type[] types)

        {
            services.AddScoped(types, typeof(IValidator<,,>));
            services.AddScoped(types, typeof(IValidator<,,,>));
            return services;
        }
        public static IServiceCollection AddHandlers(this IServiceCollection services, Type[] types)
        {
            services.AddScoped(types, typeof(ICommandHandler<>));
            services.AddScoped(types, typeof(ICommandHandler<,>));
            return services;
        }
        public static void UseCommandMediator<EStatus>(this IHost host, Dictionary<EStatus, string> errors)
            where EStatus : Enum
        {
            var _errors = host.Services.GetRequiredService<IValidationMessageData<EStatus>>();
            _errors.Messages = errors;
        }
        private static void AddScoped(this IServiceCollection services, Type[] types, Type forType)
        {
            var classes = types.GetClasses(forType);
            for (var i = 0; i < classes.Length; i++)
            {
                services.AddScoped(classes[i].GetInterfaces()[0], classes[i]);
            }
        }
    }
}
