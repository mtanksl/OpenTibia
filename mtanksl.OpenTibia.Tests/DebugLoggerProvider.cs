using OpenTibia.Game.Common.ServerObjects;
using System.Diagnostics;

namespace OpenTibia.Tests
{
    public class DebugLoggerProvider : ILoggerProvider
    {
        public void BeginWrite(LogLevel level) { }

        public void Write(string message)
        {
            Debug.Write(message);
        }

        public void EndWrite() { }
    }
}