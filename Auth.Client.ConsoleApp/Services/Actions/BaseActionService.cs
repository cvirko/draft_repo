using Auth.Client.ConsoleApp.Consts;
using Auth.Client.ConsoleApp.Interfaces.Actions;
using Auth.Client.ConsoleApp.Models.ConsoleMessages;
using Auth.Client.ConsoleApp.Models.Exceptions;

namespace Auth.Client.ConsoleApp.Services.Actions
{
    internal class BaseActionService(params IActionService[] services) : IDisposable
    {
        private SMessage[] messages = [
                new(true, "Press 'F' to exit"),
                new(true, "Press 'C' to choose action"),
            ];
        private readonly IActionService[] actions = services;
        public async Task RunAsync()
        {
            ConsoleKey key = ConsoleKey.A;

            while (key != ConsoleKey.F)
            {
                for (int i = 0; i < messages.Length; i++)
                    if (messages[i].IsVisible) Console.WriteLine(messages[i].Text);

                try
                {
                    key = Console.ReadKey().Key;
                    Console.WriteLine("\n");
                    switch (key)
                    {
                        case ConsoleKey.F:
                            break;
                        case ConsoleKey.C:
                            await ChooseActionAsync();
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
        public void Dispose()
        {
            for (int i = 0; i < actions.Length; i++)
                actions[i]?.Dispose();
        }
        private async Task ChooseActionAsync()
        {
            for (int i = 0; i < actions.Length; i++)
            {
                var a = actions[i].GetType();
                Console.WriteLine(string.Format("{0} - Press {1} to continue"
                    , actions[i].GetType().Name.Replace("ActionService", ""), i));
            }
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                await ChooseActionAsync(); return;
            }
                
            if (0 > number || number > actions.Length - 1)
            {
                await ChooseActionAsync(); return;
            }
               
            await actions[number].AddActionsAsync(ChooseActionAsync);
        }
    }
}
