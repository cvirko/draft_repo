using Auth.Client.ConsoleApp.Consts;
using Auth.Client.ConsoleApp.Interfaces;
using Auth.Client.ConsoleApp.Models;
using Auth.Domain.Core.Common.Exceptions;

namespace Auth.Client.ConsoleApp.Services.UserActions
{
    internal class ChatActionService(IUnitOfWorkServerApi _api)
    {
        private IUnitOfWorkServerApi _api = _api;
        private Message[] messages = [
                new(true, "Press 'F' to exit"),
                new(false, "Press 'D' to disconnect"),
                new(true, "Press 'J' to join"),
                new(false, "Press 'M' to send message"),
                new(true, "Press 'G' to change group name"),
            ];
        private string groupName = "test";
        public async Task AddActionsAsync()
        {
            ConsoleKey key = ConsoleKey.A;

            while (key != ConsoleKey.F)
            {
                for (int i = 0; i < messages.Length; i++)
                    if (messages[i].IsVisible) Console.WriteLine(messages[i].Text);

                key = Console.ReadKey().Key;
                Console.WriteLine("\n");
                try
                {
                    switch (key)
                    {
                        case ConsoleKey.F:
                            break;
                        case ConsoleKey.G:
                            Console.WriteLine("Input group name");
                            groupName = Console.ReadLine();
                            break;
                        case ConsoleKey.D:
                            await _api.Chat().LeaveAsync(groupName, _api.Account().Token);
                            messages[1].IsVisible = false;
                            messages[3].IsVisible = false;
                            messages[2].IsVisible = true;
                            messages[4].IsVisible = true;
                            break;
                        case ConsoleKey.J:
                            await _api.Chat().JoinAsync(groupName, _api.Account().Token);
                            messages[1].IsVisible = true;
                            messages[3].IsVisible = true;
                            messages[2].IsVisible = false;
                            messages[4].IsVisible = false;
                            break;
                        case ConsoleKey.M:
                            await _api.Chat().SendAsync(groupName, _api.Account().Token);
                            break;
                        default: break;
                    }
                }
                catch (ResponseException ex)
                {
                    if (ex.Message != null)
                        Console.WriteLine("{0}{1}{2}", ConsoleConst.BLUE, ex.Message, ConsoleConst.NORMAL);
                    foreach (var error in ex.Errors)
                        Console.WriteLine("{2}{0}: {1}{3}", error.Field, error.Title, ConsoleConst.BLUE, ConsoleConst.NORMAL);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ConsoleConst.BLUE, ex, ConsoleConst.NORMAL);
                }
            }
        }
    }
}
