using Auth.Domain.Core.Common.Consts;
using Auth.Domain.Interface.Logic.Notification.Mail;
using Auth.Domain.Interface.Logic.Notification.Sockets;
using Auth.Infrastructure.Logic.Notification.Mails;
using Auth.Infrastructure.Logic.Notification.Sockets;
using Auth.Infrastructure.Logic.Notification.Sockets.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Auth.Infrastructure.Logic.Notification
{
    public static class IoCNotificationServices
    {
        public static void RegistrationNotificationService(this IServiceCollection services)
        {
            services.AddSingleton<IMailService, SMTPMailService>();
            services.AddScoped<IChatMessageService, ChatMessageService>();

            services.AddSignalR(hubOptions =>
            {
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(30);
                hubOptions.HandshakeTimeout = TimeSpan.FromMinutes(1);
                hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
                hubOptions.MaximumParallelInvocationsPerClient = 2;
                hubOptions.StatefulReconnectBufferSize = 100_000;
            }).AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }
        public static void UseNotification(this IEndpointRouteBuilder builder)
        {
            builder.MapHub<ChatHub>($"/{AppConsts.HUBNAME}", options =>
            {
                options.Transports =
                    HttpTransportType.ServerSentEvents
                    ;
                options.TransportMaxBufferSize = 64;
                options.CloseOnAuthenticationExpiration = true;
                options.AllowStatefulReconnects = true;
                options.CloseOnAuthenticationExpiration = true;
            });
        }
    }
}
