using OpenTibia.Game;
using System;

namespace OpenTibia.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var server = new Server(7171, 7172) )
            {
#if DEBUG
                server.Logger = new Logger(new ConsoleLoggerProvider(), LogLevel.Debug);
#else
                server.Logger = new Logger(new ConsoleLoggerProvider(), LogLevel.Information);
#endif
                server.Start();

                bool exit = false;

                while ( !exit )
                {
                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "cls":
                        case "clear":

                            Console.Clear();

                            break;

                        case "":
                        case "exit":

                            exit = true;

                            break;
                    }
                }

                server.KickAll();

                server.Save();

                server.Stop();
            }

            Console.ReadKey();
        }
    }
}