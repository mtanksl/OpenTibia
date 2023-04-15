using System;
using System.Diagnostics;

namespace OpenTibia.Game
{
    public class StopwatchLogger : IDisposable
    {
        private ILoggerProvider provider;

        private LogLevel filter;

        private string message;

        private LogLevel level;

        private bool inline;

        public StopwatchLogger(ILoggerProvider provider, LogLevel filter, string message, LogLevel level, bool inline)
        {
            this.provider = provider;

            this.filter = filter;

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
                if (level >= filter)
                {
                    provider.BeginWrite(level);

                    provider.Write(message + "... ");
                }
            }
        }

        public void Dispose()
        {
            stopWatch.Stop();

            if (inline)
            {
                if (level >= filter)
                {
                    provider.Write(stopWatch.ElapsedMilliseconds + "ms" + Environment.NewLine);

                    provider.EndWrite();
                }
            }
            else
            {
                if (level >= filter)
                {
                    provider.BeginWrite(level);

                    provider.Write(message + "... " + stopWatch.ElapsedMilliseconds + "ms" + Environment.NewLine);

                    provider.EndWrite();
                }
            }
        }
    }
}