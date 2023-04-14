using System;
using System.Diagnostics;

namespace OpenTibia.Game
{
    public class StopwatchLogger : IDisposable
    {
        private ILoggerProvider provider;

        private string message;

        private LogLevel level;

        private bool inline;

        public StopwatchLogger(ILoggerProvider provider, string message, LogLevel level, bool inline)
        {
            this.provider = provider;

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
                provider.BeginWrite(level);

                provider.Write(message + "... ");
            }
        }

        public void Dispose()
        {
            stopWatch.Stop();

            if (inline)
            {
                provider.Write(stopWatch.ElapsedMilliseconds + "ms");

                provider.EndWrite();

                provider.Line();
            }
            else
            {
                provider.BeginWrite(level);

                provider.Write(message + "... " + stopWatch.ElapsedMilliseconds + "ms");

                provider.EndWrite();

                provider.Line();
            }
        }
    }
}