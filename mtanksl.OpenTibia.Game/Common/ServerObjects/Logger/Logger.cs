using System;

namespace OpenTibia.Game
{
    public class Logger
    {
        private readonly object sync = new object();

        private ILoggerProvider provider;

        private LogLevel filter;

        public Logger(ILoggerProvider provider, LogLevel filter)
        {
            this.provider = provider;

            this.filter = filter;
        }

        public StopwatchLogger Measure(string message, LogLevel level = LogLevel.Default, bool inline = true)
        {
            StopwatchLogger logger = new StopwatchLogger(provider, filter, message, level, inline);

            logger.Start();

            return logger;
        }

        public void Write(string message, LogLevel level = LogLevel.Default)
        {
            if (level >= filter)
            {
                lock (sync)
                {
                    provider.BeginWrite(level);

                    provider.Write(message);

                    provider.EndWrite();
                }
            }
        }

        public void WriteLine(string message, LogLevel level = LogLevel.Default)
        {
            if (level >= filter)
            {
                lock (sync)
                {
                    provider.BeginWrite(level);

                    provider.Write(message + Environment.NewLine);

                    provider.EndWrite();
                }
            }
        }

        public void WriteLine(LogLevel level = LogLevel.Default)
        {
            if (level >= filter)
            {
                lock (sync)
                {
                    provider.BeginWrite(level);

                    provider.Write(Environment.NewLine);

                    provider.EndWrite();
                }
            }
        }
    }
}