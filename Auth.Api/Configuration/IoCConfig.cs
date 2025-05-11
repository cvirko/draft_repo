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
            service.Add<RabbitMQOptions>(configuration, AppConsts.RABBITMQ_SECTION_NAME);
            service.Add<PaymentOptions>(configuration, AppConsts.PAYMENT_SECTION_NAME);
        }
        public static void AddIoC(this IHostApplicationBuilder builder)
        {
            builder.Services.RegistrationValidationService();
            builder.Services.RegistrationReadModelService();
            builder.Services.RegistrationWriteService();
            builder.Services.RegistrationExternalService(
                builder.Configuration.Get<PaymentOptions>(AppConsts.PAYMENT_SECTION_NAME));
            builder.Services.RegistrationNotificationService();
            builder.Services.AddAuthentication(
                builder.Configuration.Get<TokenOptions>(AppConsts.TOKEN_SETTING_SECTION_NAME),
                builder.Configuration.Get<AuthOptions>(AppConsts.AUTH_SETTING_SECTION_NAME));
            builder.Services.RegistrationDataServices(builder.Environment, builder.Configuration.Get<ConnectionOptions>(AppConsts.CONNECTION_SECTION_NAME));
            builder.Services.AddWorkers();
        }
        public static void UseIoC(this WebApplication app, IHostApplicationBuilder builder)
        {
            var option = builder.Configuration.Get<FilesOptions>(AppConsts.FILE_SECTION_NAME);
            string webRootPath = Path.Combine(builder.Environment.ContentRootPath,"wwwroot");

            app.UseValidationMessages(option);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(webRootPath, option.AvatarsStorePath)),
                RequestPath = AppConsts.AVATARS_PATH
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(webRootPath, option.VideoStorePath)),
                RequestPath = AppConsts.VIDEO_PATH,
                OnPrepareResponse = ctx =>
                {
                    if (!ctx.Context.User.Identity.IsAuthenticated)
                    {
                        ctx.Context.Response.StatusCode = 401;
                        ctx.Context.Response.ContentLength = 0;
                        ctx.Context.Response.Body = Stream.Null;
                    }
                }
            }
            );

            app.UseNotification();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseDataServices();
        }
        private static void Add<T>(this IServiceCollection service, IConfiguration configuration, string sectionName) where T : Options
        {
            service.Configure<T>(configuration.GetSection(sectionName));

        }
        private static T Get<T>(this IConfiguration configuration, string name) => configuration.GetSection(name).Get<T>();
    }
}
