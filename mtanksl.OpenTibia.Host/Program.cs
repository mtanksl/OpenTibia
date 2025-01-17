using Microsoft.EntityFrameworkCore;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
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

            logger.WriteLine("Available commands: help, clear, reload-plugins, broadcast, maintenance, kick, save, clean, stop, stats, about.");
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

                                logger.WriteLine("clear \t\t Clear the console screen. Alternative commands: cls.");
                                logger.WriteLine("reload-plugins \t Reload plugins.");
                                logger.WriteLine("broadcast \t Broadcast message to all the players.");
                                logger.WriteLine("maintenance \t Start or stop the server maintenance.");
                                logger.WriteLine("kick \t\t Kick all the players.");
                                logger.WriteLine("save \t\t Save the server.");
                                logger.WriteLine("clean \t\t Clean the server.");
                                logger.WriteLine("stop \t\t Stop the server. Alternative commands: exit.");
                                logger.WriteLine("stats \t\t Display server statistics.");
                                logger.WriteLine("about \t\t Display license.");
                                logger.WriteLine();

                                break;

                            case "clear":
                            case "cls":

                                Console.Clear();

                                break;

                            case "reload-plugins":

                                server.ReloadPlugins();

                                break;

                            case "broadcast":

                                logger.Write("Message: ");

                                string message = Console.ReadLine();

                                if ( !string.IsNullOrEmpty(message) )
                                {
                                    server.QueueForExecution( () =>
                                    {
                                        ShowWindowTextOutgoingPacket showTextOutgoingPacket = new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, message);

                                        foreach (var observer in Context.Current.Server.GameObjects.GetPlayers() )
                                        {
                                            Context.Current.AddPacket(observer, showTextOutgoingPacket);
                                        }

                                        return Promise.Completed;

                                    } ).Wait();
                                }

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

                            case "clean":

                                server.Clean();

                                break;

                            case null:
                            case "stop":
                            case "exit":

                                exit = true;

                                break;
                                                                
                            case "stats":

                                logger.WriteLine("Uptime: " + server.Statistics.Uptime.Days + " days " + server.Statistics.Uptime.Hours + " hours " + server.Statistics.Uptime.Minutes + " minutes", LogLevel.Information);
                                logger.WriteLine("Players peek: " + server.Statistics.PlayersPeek, LogLevel.Information);
                                logger.WriteLine("Active connections: " + server.Statistics.ActiveConnections, LogLevel.Information);
                                logger.WriteLine("Total messages sent: " + server.Statistics.TotalMessagesSent, LogLevel.Information);
                                logger.WriteLine("Total bytes sent: " + server.Statistics.TotalBytesSent + " bytes (" + ConvertBytesToHumanReadable(server.Statistics.TotalBytesSent) + ")", LogLevel.Information);
                                logger.WriteLine("Total messages received: " + server.Statistics.TotalMessagesReceived, LogLevel.Information);
                                logger.WriteLine("Total bytes received: " + server.Statistics.TotalBytesReceived + " bytes (" + ConvertBytesToHumanReadable(server.Statistics.TotalBytesReceived) + ")", LogLevel.Information);
                                logger.WriteLine("Average processing time: " + server.Statistics.AverageProcessingTime.ToString("N3") + " milliseconds (" + (1000 / server.Statistics.AverageProcessingTime).ToString("N0") + " FPS)", LogLevel.Information);
                                logger.WriteLine();

                                break;

                            case "about":

                                logger.WriteLine("MTOTS - An open Tibia server developed by mtanksl");
                                logger.WriteLine("Copyright (C) 2024 mtanksl");
                                logger.WriteLine();
                                logger.WriteLine("This program is free software: you can redistribute it and/or modify");
                                logger.WriteLine("it under the terms of the GNU General Public License as published by");
                                logger.WriteLine("the Free Software Foundation, either version 3 of the License, or");
                                logger.WriteLine("(at your option) any later version.");
                                logger.WriteLine();
                                logger.WriteLine("This program is distributed in the hope that it will be useful,");
                                logger.WriteLine("but WITHOUT ANY WARRANTY; without even the implied warranty of");
                                logger.WriteLine("MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the");
                                logger.WriteLine("GNU General Public License for more details.");
                                logger.WriteLine();
                                logger.WriteLine("You should have received a copy of the GNU General Public License");
                                logger.WriteLine("along with this program. If not, see <https://www.gnu.org/licenses/>.");
                                logger.WriteLine();

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