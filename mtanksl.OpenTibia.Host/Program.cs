using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Threading.Tasks;

namespace mtanksl.OpenTibia.Host
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var tcs = new TaskCompletionSource<string>();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;

                tcs.SetResult(null);
            };
            
#if DEBUG
            ILogger logger = new Logger(new ConsoleLoggerProvider(), LogLevel.Debug);
#else
            ILogger logger = new Logger(new ConsoleLoggerProvider(), LogLevel.Information);
#endif

            logger.WriteLine("Available commands: help, stats, clear, reload-plugins, maintenance, kick, save, stop.");

            logger.WriteLine();

            try
            {
                using (var server = new Server() )
                {
                    server.Logger = logger;

                    server.Start();

                    bool exit = false;

                    while ( !exit )
                    {
                        var option = await await Task.WhenAny(tcs.Task, Task.Run( () => Console.ReadLine() ) );
                        
                        switch (option)
                        {
                            case "help":

                                server.Logger.WriteLine("stats \t\t Display server statistics.");
                                server.Logger.WriteLine("clear \t\t Clear the console screen. Alternative commands: cls.");
                                server.Logger.WriteLine("reload-plugins \t Reload plugins.");
                                server.Logger.WriteLine("maintenance \t Start or stop the server maintenance.");
                                server.Logger.WriteLine("kick \t\t Kick all the players.");
                                server.Logger.WriteLine("save \t\t Save the server.");
                                server.Logger.WriteLine("stop \t\t Stop the server. Alternative commands: exit, quit.");

                                break;

                            case "stats":

                                server.Logger.WriteLine("Uptime: " + server.Statistics.Uptime.Days + " days " + server.Statistics.Uptime.Hours + " hours " + server.Statistics.Uptime.Minutes + " minutes", LogLevel.Information);
                                server.Logger.WriteLine("Players peek: " + server.Statistics.PlayersPeek, LogLevel.Information);
                                server.Logger.WriteLine("Active connections: " + server.Statistics.ActiveConnections, LogLevel.Information);
                                server.Logger.WriteLine("Total messages sent: " + server.Statistics.TotalMessagesSent, LogLevel.Information);
                                server.Logger.WriteLine("Total bytes sent: " + server.Statistics.TotalBytesSent + " bytes (" + ConvertBytesToHumanReadable(server.Statistics.TotalBytesSent) + ")", LogLevel.Information);
                                server.Logger.WriteLine("Total messages received: " + server.Statistics.TotalMessagesReceived, LogLevel.Information);
                                server.Logger.WriteLine("Total bytes received: " + server.Statistics.TotalBytesReceived + " bytes (" + ConvertBytesToHumanReadable(server.Statistics.TotalBytesReceived) + ")", LogLevel.Information);
                                server.Logger.WriteLine("Average processing time: " + server.Statistics.AverageProcessingTime.ToString("N3") + " milliseconds (" + (1000 / server.Statistics.AverageProcessingTime).ToString("N0") + " FPS)", LogLevel.Information);

                                break;

                            case "clear":
                            case "cls":

                                Console.Clear();

                                break;

                            case "reload-plugins":

                                server.ReloadPlugins();

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

                            case "kick":

                                server.KickAll();

                                break;

                            case "save":

                                server.Save();

                                break;

                            case null:
                            case "stop":
                            case "exit":
                            case "quit":

                                exit = true;

                                break;
                        }
                    }

                    server.KickAll();

                    server.Save();

                    server.Stop();
                }
            }
            catch (Exception ex)
            {
                logger.WriteLine(ex.ToString(), LogLevel.Error);

                return 1;
            }

            return 0;
        }

        private static readonly string[] Sizes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        private static string ConvertBytesToHumanReadable(ulong bytes)
        {
            double size = bytes;

            int magnitude = 0;

            while (size > 1024)
            {
                magnitude++;

                size /= 1024;
            }

            return size.ToString("0.00") + " " + Sizes[magnitude];
        }
    }
}