using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Logic.External;
using Auth.Infrastructure.Logic.Notification;
using Auth.Infrastructure.Logic.Read;
using Auth.Infrastructure.Logic.Validation;
using Auth.Infrastructure.Logic.Write;
using Microsoft.Extensions.FileProviders;

namespace Auth.Api.Configuration
{
    public static class IoCConfig
    {
        public static void RegistrationConfigureSections(this IServiceCollection service, IConfiguration configuration)
        {
            service.Add<TokenOptions>(configuration, AppConsts.TOKEN_SETTING_SECTION_NAME);
            service.Add<ConnectionOptions>(configuration, AppConsts.CONNECTION_SECTION_NAME);
            service.Add<FilesOptions>(configuration, AppConsts.FILE_SECTION_NAME);
            service.Add<CacheOptions>(configuration, AppConsts.CACHE_SECTION_NAME);
            service.Add<MailOptions>(configuration, AppConsts.MAIL_SECTION_NAME);
            service.Add<FailedAccessOptions>(configuration, AppConsts.USER_FAILED_ACCESS_SECTION_NAME);
            service.Add<AuthOptions>(configuration, AppConsts.AUTH_SETTING_SECTION_NAME);
        }
        public static void AddIoC(this IHostApplicationBuilder builder)
        {
            builder.Services.RegistrationValidationService();
            builder.Services.RegistrationReadModelService();
            builder.Services.RegistrationWriteService();
            builder.Services.RegistrationExternalService();
            builder.Services.RegistrationNotificationService();
            builder.Services.AddAuthentication(
                builder.Configuration.Get<TokenOptions>(AppConsts.TOKEN_SETTING_SECTION_NAME),
                builder.Configuration.Get<AuthOptions>(AppConsts.AUTH_SETTING_SECTION_NAME));
            builder.Services.RegistrationDBService(builder.Environment, builder.Configuration.Get<ConnectionOptions>(AppConsts.CONNECTION_SECTION_NAME));

        }
        public static void UseIoC(this WebApplication app, IHostApplicationBuilder builder)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, 
                builder.Configuration.Get<FilesOptions>(AppConsts.FILE_SECTION_NAME).AvatarsStorePath)),
                RequestPath = $"/{AppConsts.AVATARS_PATH}"
            });

            app.UseNotification();

            app.UseAuthentication();

            app.UseAuthorization();
        }
        private static void Add<T>(this IServiceCollection service, IConfiguration configuration, string sectionName) where T : Options
        {
            service.Configure<T>(configuration.GetSection(sectionName));

        }
        private static T Get<T>(this IConfiguration configuration, string name) => configuration.GetSection(name).Get<T>();
    }
}
