using OpenTibia.Game;
using System;
using System.IO;

namespace OpenTibia.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if ( !File.Exists("data\\database.db") )
            {
                File.Copy("data\\template.db", "data\\database.db");
            }

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
                    var option = Console.ReadLine();

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

                server.Stop();
            }

            Console.ReadKey();
        }
    }
}