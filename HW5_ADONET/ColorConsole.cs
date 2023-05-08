namespace HW_ADONET__EFCore
{
    internal static class ColorConsole
    {
        public static void Write(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            Console.Write(text);

            Console.ResetColor();
        }

        public static void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            Console.WriteLine(text);

            Console.ResetColor();
        }
    }
}
