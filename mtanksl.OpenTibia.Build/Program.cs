using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace mtanksl.OpenTibia.Build
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

            WriteLine("Available commands: help, publish, migration, clear, exit, about.");
            WriteLine();

            try
            {
                bool exit = false;

                while ( !exit )
                {
                    var option = await await Task.WhenAny(tcs.Task, Task.Run( () => Console.ReadLine() ) );
                        
                    switch (option)
                    {
                        case "help":

                            WriteLine("publish\t\tRebuild the solution, run tests, publish and zip.");
                            WriteLine("migration\t\tCreate migration scripts for SQLite, MySQL, Microsoft SQL Server and Postgre SQL.");
                            WriteLine("clear\t\tClear the console screen. Alternative commands: cls.");
                            WriteLine("exit\t\tExit this build tool.");
                            WriteLine("about\t\tDisplay license.");
                            WriteLine();

                            break;

                        case "publish":

                            WriteLine("Remember to update server, console and gui versions.", LogLevel.Information);

                            WriteLine("Running tests...", LogLevel.Information);

                                int exitCode = Run(
                                    "mtanksl.OpenTibia.Tests", 
                                    "dotnet", "test mtanksl.OpenTibia.Tests.csproj -c Release");

                                if (exitCode != 0)
                                {
                                    WriteLine("Could not test mtanksl.OpenTibia.Tests, exited with code " + exitCode, LogLevel.Error);

                                    break;
                                }

                            WriteLine("Building plugins...", LogLevel.Information);

                                DirectoryDelete(
                                    "mtanksl.OpenTibia.Plugins\\bin\\Release\\netstandard2.0");

                                exitCode = Run(
                                    "mtanksl.OpenTibia.Plugins", 
                                    "dotnet", "build mtanksl.OpenTibia.Plugins.csproj -c Release");

                                if (exitCode != 0)
                                {
                                    WriteLine("Could not build mtanksl.OpenTibia.Plugins, exited with code " + exitCode, LogLevel.Error);

                                    break;
                                }

                                FileCopy(
                                    "mtanksl.OpenTibia.Plugins\\bin\\Release\\netstandard2.0\\mtanksl.OpenTibia.Plugins.dll", 
                                    "mtanksl.OpenTibia.GameData\\data\\dlls\\mtanksl.OpenTibia.Plugins\\mtanksl.OpenTibia.Plugins.dll");

                            WriteLine("Building Windows (console) application...", LogLevel.Information);
                             
                                DirectoryDelete(
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\win-x64\\publish");

                                exitCode = Run(
                                    "mtanksl.OpenTibia.Host", 
                                    "dotnet", "publish mtanksl.OpenTibia.Host.csproj -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false");

                                if (exitCode != 0)
                                {
                                    WriteLine("Could not publish mtanksl.OpenTibia.Host for win-x64, exited with code " + exitCode, LogLevel.Error);

                                    break;
                                }

                                FileDeletePdb(
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\win-x64\\publish");

                                DirectoryCopy(
                                    "mtanksl.OpenTibia.GameData\\data",
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\win-x64\\publish\\data");

                                Zip(
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\win-x64\\publish",
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\win-x64\\mtanksl.OpenTibia.Host_win-x64.zip");

                            WriteLine("Building Linux (console) application...", LogLevel.Information);

                                DirectoryDelete(
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\linux-x64\\publish");

                                exitCode = Run(
                                    "mtanksl.OpenTibia.Host",
                                    "dotnet", "publish mtanksl.OpenTibia.Host.csproj -c Release -r linux-x64 -p:PublishSingleFile=true --self-contained true");

                                if (exitCode != 0)
                                {
                                    WriteLine("Could not publish mtanksl.OpenTibia.Host for linux-x64, exited with code " + exitCode, LogLevel.Error);

                                    break;
                                }

                                FileDeletePdb(
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\linux-x64\\publish");

                                DirectoryCopy(
                                    "mtanksl.OpenTibia.GameData\\data", 
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\linux-x64\\publish\\data");

                                DirectoryDelete(
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\linux-x64\\publish\\data\\clibs");

                                DirectoryDelete(
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\linux-x64\\publish\\data\\lualibs");

                                DirectoryCreate(
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\linux-x64\\publish\\data\\clibs");

                                DirectoryCreate(
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\linux-x64\\publish\\data\\lualibs");

                                Zip(
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\linux-x64\\publish",
                                    "mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\linux-x64\\mtanksl.OpenTibia.Host_linux-x64.zip");

                            WriteLine("Building Windows (GUI) application...", LogLevel.Information);

                                DirectoryDelete(
                                    "mtanksl.OpenTibia.Host.GUI\\bin\\Release\\net8.0-windows\\win-x64\\publish");

                                exitCode = Run(
                                    "mtanksl.OpenTibia.Host.GUI", 
                                    "dotnet", "publish mtanksl.OpenTibia.Host.GUI.csproj -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false");

                                if (exitCode != 0)
                                {
                                    WriteLine("Could not publish mtanksl.OpenTibia.Host.GUI for win-x64, exited with code " + exitCode, LogLevel.Error);

                                    break;
                                }

                                FileDeletePdb(
                                    "mtanksl.OpenTibia.Host.GUI\\bin\\Release\\net8.0-windows\\win-x64\\publish");

                                DirectoryCopy(
                                    "mtanksl.OpenTibia.GameData\\data", 
                                    "mtanksl.OpenTibia.Host.GUI\\bin\\Release\\net8.0-windows\\win-x64\\publish\\data");

                                Zip(
                                    "mtanksl.OpenTibia.Host.GUI\\bin\\Release\\net8.0-windows\\win-x64\\publish",
                                    "mtanksl.OpenTibia.Host.GUI\\bin\\Release\\net8.0-windows\\win-x64\\mtanksl.OpenTibia.Host.GUI_win-x64.zip");

                            WriteLine("Publish completed... 3 files were created:");
                            WriteLine("mtanksl.OpenTibia\\mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\win-x64\\mtanksl.OpenTibia.Host_win-x64.zip");
                            WriteLine("mtanksl.OpenTibia\\mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\linux-x64\\mtanksl.OpenTibia.Host_linux-x64.zip");
                            WriteLine("mtanksl.OpenTibia\\mtanksl.OpenTibia.Host.GUI\\bin\\Release\\net8.0-windows\\win-x64\\mtanksl.OpenTibia.Host.GUI_win-x64.zip");
                            WriteLine();

                            WriteLine("mtanksl.OpenTibia.Host_win-x64.zip");
                            WriteLine(Hash("mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\win-x64\\mtanksl.OpenTibia.Host_win-x64.zip") );
                            WriteLine();

                            WriteLine("mtanksl.OpenTibia.Host_linux-x64.zip");
                            WriteLine(Hash("mtanksl.OpenTibia.Host\\bin\\Release\\net8.0\\linux-x64\\mtanksl.OpenTibia.Host_linux-x64.zip") );
                            WriteLine();

                            WriteLine("mtanksl.OpenTibia.Host.GUI_win-x64.zip");
                            WriteLine(Hash("mtanksl.OpenTibia.Host.GUI\\bin\\Release\\net8.0-windows\\win-x64\\mtanksl.OpenTibia.Host.GUI_win-x64.zip") );
                            WriteLine();
                            
                            break;

                        case "migration":

                            //TODO

                            break;

                        case "clear":
                        case "cls":

                            Console.Clear();

                            break;

                        case null:
                        case "exit":

                            exit = true;

                            break;

                        case "about":

                            WriteLine("MTOTS - An open Tibia server developed by mtanksl");
                            WriteLine("Copyright (C) 2024 mtanksl");
                            WriteLine();
                            WriteLine("This program is free software: you can redistribute it and/or modify");
                            WriteLine("it under the terms of the GNU General Public License as published by");
                            WriteLine("the Free Software Foundation, either version 3 of the License, or");
                            WriteLine("(at your option) any later version.");
                            WriteLine();
                            WriteLine("This program is distributed in the hope that it will be useful,");
                            WriteLine("but WITHOUT ANY WARRANTY; without even the implied warranty of");
                            WriteLine("MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the");
                            WriteLine("GNU General Public License for more details.");
                            WriteLine();
                            WriteLine("You should have received a copy of the GNU General Public License");
                            WriteLine("along with this program. If not, see <https://www.gnu.org/licenses/>.");
                            WriteLine();

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString(), LogLevel.Error);

                return 1;
            }

            return 0;
        }

        private enum LogLevel
        {
            Debug,

            Information,

            Warning,

            Error,

            Default
        }

        private static readonly object sync = new object();

        private static void WriteLine(string message = null, LogLevel logLevel = LogLevel.Default)
        {
            lock (sync)
            {
                switch (logLevel)
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

                Console.WriteLine(message);

                Console.ResetColor();
            }
        }

        private static void DirectoryDelete(string directoryName)
        {
            directoryName = Path.Combine(GetBaseDirectory(), directoryName);

            Directory.Delete(directoryName, true);
        }

        private static void FileCopy(string sourceFileName, string destinationFileName)
        {
            sourceFileName = Path.Combine(GetBaseDirectory(), sourceFileName);
            destinationFileName = Path.Combine(GetBaseDirectory(), destinationFileName);

            File.Copy(sourceFileName, destinationFileName, true);
        }

        private static void DirectoryCopy(string sourceDirectoryName, string destinationDirectoryName)
        {
            sourceDirectoryName = Path.Combine(GetBaseDirectory(), sourceDirectoryName);
            destinationDirectoryName = Path.Combine(GetBaseDirectory(), destinationDirectoryName);

            CopyEachItem(sourceDirectoryName, destinationDirectoryName);

            void CopyEachItem(string source, string destination)
            {
                Directory.CreateDirectory(destination);

                foreach (var file in Directory.GetFiles(source) )
                {
                    File.Copy(file, Path.Combine(destination, Path.GetFileName(file) ) );
                }

                foreach (var directory in Directory.GetDirectories(source) )
                {
                    CopyEachItem(directory, Path.Combine(destination, Path.GetFileName(directory) ) );
                }
            }
        }

        private static void FileDeletePdb(string directoryName)
        {
            directoryName = Path.Combine(GetBaseDirectory(), directoryName);

            foreach (var file in Directory.GetFiles(directoryName, "*.pdb" ) )
            {
                File.Delete(file);
            }
        }

        private static void DirectoryCreate(string directoryName)
        {
            directoryName = Path.Combine(GetBaseDirectory(), directoryName);

            Directory.CreateDirectory(directoryName);
        }

        private static void Zip(string directoryName, string fileName)
        {
            directoryName = Path.Combine(GetBaseDirectory(), directoryName);
            fileName = Path.Combine(GetBaseDirectory(), fileName);

            if (File.Exists(fileName) )
            {
                File.Delete(fileName);
            }

            ZipFile.CreateFromDirectory(directoryName, fileName, CompressionLevel.SmallestSize, false);
        }

        private static string Hash(string fileName)
        {
            fileName = Path.Combine(GetBaseDirectory(), fileName);

            using (var sha256 = SHA256.Create() )
            {
                using (var fileStream = File.OpenRead(fileName) )
                {
                    return Convert.ToHexString(sha256.ComputeHash(fileStream) );
                }
            }
        }

        private static int Run(string workingDirectory, string command, string arguments)
        {
            workingDirectory = Path.Combine(GetBaseDirectory(), workingDirectory);

            using (var process = new Process() )
            {
                process.StartInfo.WorkingDirectory = workingDirectory;

                process.StartInfo.FileName = command;

                process.StartInfo.Arguments = arguments;

                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.RedirectStandardError = true;

                process.StartInfo.CreateNoWindow = true;

                process.OutputDataReceived += (s, e) =>
                {
                    WriteLine(e.Data, LogLevel.Debug);
                };

                process.ErrorDataReceived += (s, e) =>
                {
                    WriteLine(e.Data, LogLevel.Debug);
                };

                process.Start();

                process.BeginOutputReadLine();

                process.BeginErrorReadLine();   
                
                process.WaitForExit();

                return process.ExitCode;
            }
        }

        private static string GetBaseDirectory()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory != null)
            {
                if (directory.Name == "mtanksl.OpenTibia")
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }

            throw new NotImplementedException();
        }
    }
}