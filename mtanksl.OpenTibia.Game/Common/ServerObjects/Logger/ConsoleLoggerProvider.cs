using System;

namespace OpenTibia.Game
{
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        public void BeginWrite(LogLevel level)
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
        }

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void EndWrite()
        {
            
        }

        public void Line()
        {
            Console.WriteLine();
        }
    }
}