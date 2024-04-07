using OpenTibia.Game;
using System;

namespace OpenTibia.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Available commands: help, cls, kick, save, maintenance, exit");
            Console.WriteLine();

            using (var server = new Server() )
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
                        case "help":

                            Console.WriteLine("cls \t\t Clears the console screen.");
                            Console.WriteLine("kick \t\t Kicks all the players.");
                            Console.WriteLine("save \t\t Saves the server.");
                            Console.WriteLine("maintenance \t Starts or stops the server maintenance.");
                            Console.WriteLine("exit \t\t Stops the server.");
                            Console.WriteLine();

                            break;

                        case "cls":
                        case "clear":

                            Console.Clear();

                            break;

                        case "kick":

                            server.KickAll();

                            break;

                        case "save":

                            server.Save();

                            break;

                        case "maintenance":

                            if (server.Status == ServerStatus.Running)
                            {
                                server.Pause();
                            }
                            else if (server.Status == ServerStatus.Paused)
                            {
                                server.Continue();
                            }

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
        }
    }
}