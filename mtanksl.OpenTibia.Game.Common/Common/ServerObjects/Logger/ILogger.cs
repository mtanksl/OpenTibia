namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ILogger
    {
        StopwatchLogger Measure(string message, LogLevel level = LogLevel.Default, bool inline = true);

        void Write(string message, LogLevel level = LogLevel.Default);

        void WriteLine(string message, LogLevel level = LogLevel.Default);

        void WriteLine(LogLevel level = LogLevel.Default);
    }
}