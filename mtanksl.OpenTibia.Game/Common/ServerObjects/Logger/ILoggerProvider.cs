namespace OpenTibia.Game
{
    public interface ILoggerProvider
    {
        void BeginWrite(LogLevel level);

        void Write(string message);

        void EndWrite();

        void Line();
    }
}