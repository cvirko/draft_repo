namespace Auth.Client.ConsoleApp.Extensions
{
    public static class ConsoleExtension 
    {
        public static void Errors(params string[] msgs)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < msgs.Length; i++)
                Console.Error.WriteLine(msgs[i]);
            Console.ResetColor();
        }
        public static void Info<T>(T msg, ConsoleColor color = ConsoleColor.Green)
        {
            Console.ForegroundColor = color;
            Console.Error.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
