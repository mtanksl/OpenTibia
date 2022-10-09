using System;
using System.Diagnostics;

namespace OpenTibia.Game
{
    public class Logger
    {
        public class StopWatchWrapper : IDisposable
        {
            private Logger logger;

            private string message;

            private LogLevel level;

            private bool inline;
                      
            public StopWatchWrapper(Logger logger, string message, LogLevel level, bool inline)
            {
                this.logger = logger;

                this.message = message;

                this.level = level;

                this.inline = inline;
            }
            
            private Stopwatch stopWatch;

            public void Start()
            {
                stopWatch = new Stopwatch();

                stopWatch.Start();

                if (inline)
                {
                    logger.Write(message + "... ", level);
                }
            }

            public void Dispose()
            {
                stopWatch.Stop();

                if (inline)
                {
                    logger.WriteLine(stopWatch.ElapsedMilliseconds + "ms", level);
                }
                else
                {
                    logger.WriteLine(message + "... " + stopWatch.ElapsedMilliseconds + "ms", level);
                }
            }
        }

        public StopWatchWrapper Measure(string message, LogLevel level = LogLevel.Default, bool inline = true)
        {
            StopWatchWrapper wrapper = new StopWatchWrapper(this, message, level, inline);

            wrapper.Start();

            return wrapper;
        }

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