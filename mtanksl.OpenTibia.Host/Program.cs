using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;

namespace OpenTibia.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Available commands: help, stats, clear, reload-plugins, kick, save, maintenance, stop.");
            Console.WriteLine();

            try
            {
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

                                Console.WriteLine("stats \t\t Displays server statistics.");
                                Console.WriteLine("clear \t\t Clears the console screen. Alternative commands: cls.");
                                Console.WriteLine("reload-plugins \t\t Reloads plugins.");
                                Console.WriteLine("kick \t\t Kicks all the players.");
                                Console.WriteLine("save \t\t Saves the server.");
                                Console.WriteLine("maintenance \t Starts or stops the server maintenance.");
                                Console.WriteLine("stop \t\t Stops the server. Alternative commands: exit, quit.");
                                Console.WriteLine();

                                break;

                            case "stats":

                                server.Logger.WriteLine("Uptime: " + (int)server.Statistics.Uptime.TotalMinutes + " minutes", LogLevel.Information);
                                server.Logger.WriteLine("Active connections: " + server.Statistics.ActiveConnections, LogLevel.Information);
                                server.Logger.WriteLine("Total messages sent: " + server.Statistics.TotalMessagesSent, LogLevel.Information);
                                server.Logger.WriteLine("Total bytes sent: " + server.Statistics.TotalBytesSent + " bytes", LogLevel.Information);
                                server.Logger.WriteLine("Total messages received: " + server.Statistics.TotalMessagesReceived, LogLevel.Information);
                                server.Logger.WriteLine("Total bytes received: " + server.Statistics.TotalBytesReceived + " bytes", LogLevel.Information);
                                server.Logger.WriteLine("Average processing time: " + server.Statistics.AverageProcessingTime.ToString("N3") + " milliseconds", LogLevel.Information);

                                break;

                            case "cls":
                            case "clear":

                                Console.Clear();

                                break;

                            case "reload-plugins":

                                server.ReloadPlugins();

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
                            case "quit":
                            case "stop":

                                exit = true;

                                break;
                        }
                    }

                    server.KickAll();

                    server.Save();

                    server.Stop();
                }
            }
            catch { }

            Console.ReadKey();
        }
    }
}