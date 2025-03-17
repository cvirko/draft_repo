using Auth.Client.ConsoleApp.Consts;
using Auth.Client.ConsoleApp.Interfaces;
using Auth.Domain.Core.Logic.Models.Hub;
using Auth.Domain.Interface.Logic.Notification.Sockets.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Auth.Client.ConsoleApp.Services.SiganlR
{
    internal class SignalRClient : IDisposable
    {
        public SignalRClient(string uri, IUnitOfWorkServerApi unitOfWorkServer)
        {
            Create(uri, unitOfWorkServer);
        }
        private HubConnection hubConnection = default;
        private void Create(string uri, IUnitOfWorkServerApi server)
        {
            hubConnection = new HubConnectionBuilder()
                
                .WithUrl(uri, option =>
                {
                    option.AccessTokenProvider = () => Task.FromResult(server.Account().Token);
                    option.UseDefaultCredentials = true;
                    option.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.ServerSentEvents;
                })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .WithServerTimeout(TimeSpan.FromSeconds(60 * 2))
                .WithKeepAliveInterval(TimeSpan.FromSeconds(10))
                .WithStatefulReconnect()
                .WithAutomaticReconnect([TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30)])
                .Build();
        }
        public async Task ConnectToMessageListener()
        {
            if (hubConnection == null) throw new ArgumentNullException("NO {0}", nameof(hubConnection));
            try
            {
                var name = nameof(IChatHubClient.ReceiveChatMessageAsync);
                hubConnection.On<UserMessageRequest>(name,
                stock => Console.WriteLine("{0}{1}: {2}{3}",
                    ConsoleConst.GREEN, stock.Sender, stock.Message, ConsoleConst.NORMAL));

                await hubConnection.StartAsync();

                Console.WriteLine("{0}Connection started{1}", ConsoleConst.YELLOW, ConsoleConst.NORMAL);

                hubConnection.Closed += (exception) =>
                {
                    Console.WriteLine("{0}Disconnection {2}{1}",
                        ConsoleConst.YELLOW, ConsoleConst.NORMAL, exception);
                    return Task.CompletedTask;
                };
                hubConnection.Reconnecting += (exception) =>
                {
                    Console.WriteLine("{0}Reconnecting {2}{1}",
                        ConsoleConst.YELLOW, ConsoleConst.NORMAL, exception);
                    return Task.CompletedTask;
                };
                hubConnection.Reconnected += (exception) =>
                {
                    Console.WriteLine("{0}Connected {2}{1}",
                        ConsoleConst.YELLOW, ConsoleConst.NORMAL, exception);
                    return Task.CompletedTask;
                };
            }
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.Unauthorized:
                    case System.Net.HttpStatusCode.Forbidden:
                        Console.WriteLine("{0}Disconnection {2}{1}",
                            ConsoleConst.YELLOW, ConsoleConst.NORMAL, ex.StatusCode.ToString());
                        break;
                    default:
                        Console.WriteLine("{0}Disconnection {2}{1}",
                            ConsoleConst.YELLOW, ConsoleConst.NORMAL, ex.StatusCode.ToString());
                        throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}Disconnection {2}{1}", ConsoleConst.BLUE, ConsoleConst.NORMAL, ex);
            }
        }
        public void Dispose()
        {
            _ = hubConnection.DisposeAsync();
        }
    }
}
