using System;

namespace mtanksl.OpenTibia.Proxy
{
    public class Logger
    {
        public void Write(string message, LogLevel level = LogLevel.Default)
        {
            switch (level)
            {
                case LogLevel.Debug:

                    Console.ForegroundColor = ConsoleColor.Green;

                    break;

                case LogLevel.Information:

                    Console.ForegroundColor = ConsoleColor.Blue;

                    break;

                case LogLevel.Warning:

                    Console.ForegroundColor = ConsoleColor.DarkYellow;

                    break;

                case LogLevel.Error:

                    Console.ForegroundColor = ConsoleColor.Red;

                    break;

                default:

                    Console.ResetColor();

                    break;
            }

            Console.Write(message);
        }

        public void WriteLine(string message, LogLevel level = LogLevel.Default)
        {
            Write(message, level);

            Console.WriteLine();
        }
    }

    public enum LogLevel
    {
        Default,

        Debug,

        Information,

        Warning,

        Error
    }
}
