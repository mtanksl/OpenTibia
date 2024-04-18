namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ILoggerProvider
    {
        void BeginWrite(LogLevel level);

        void Write(string message);

        void EndWrite();
    }
}