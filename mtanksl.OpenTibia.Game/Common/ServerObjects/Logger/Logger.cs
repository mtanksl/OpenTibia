namespace OpenTibia.Game
{
    public class Logger
    {
        private ILoggerProvider provider;

        public Logger(ILoggerProvider provider)
        {
            this.provider = provider;
        }

        public StopwatchLogger Measure(string message, LogLevel level = LogLevel.Default, bool inline = true)
        {
            StopwatchLogger logger = new StopwatchLogger(provider, message, level, inline);

            logger.Start();

            return logger;
        }

        public void WriteLine(string message, LogLevel level = LogLevel.Default)
        {
            provider.BeginWrite(level);

            provider.Write(message);

            provider.EndWrite();

            provider.Line();
        }

        public void Write(string message, LogLevel level = LogLevel.Default)
        {
            provider.BeginWrite(level);

            provider.Write(message);

            provider.EndWrite();
        }

        public void WriteLine()
        {
            provider.Line();
        }
    }
}