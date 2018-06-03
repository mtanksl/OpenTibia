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

            private bool inline;
                      
            public StopWatchWrapper(Logger logger, string message, bool inline)
            {
                this.logger = logger;

                this.message = message;

                this.inline = inline;
            }
            
            private Stopwatch stopWatch;

            public void Start()
            {
                stopWatch = new Stopwatch();

                stopWatch.Start();

                if (inline)
                {
                    logger.Write(message + "... ");
                }
            }

            public void Dispose()
            {
                stopWatch.Stop();

                if (inline)
                {
                    logger.WriteLine(stopWatch.ElapsedMilliseconds + "ms");
                }
                else
                {
                    logger.WriteLine(message + "... " + stopWatch.ElapsedMilliseconds + "ms");
                }
            }
        }

        public StopWatchWrapper Measure(string message, bool inline)
        {
            StopWatchWrapper wrapper = new StopWatchWrapper(this, message, inline);

            wrapper.Start();

            return wrapper;
        }

        public virtual void Write(string message, params object[] arguments)
        {
            Console.Write(message, arguments);
        }

        public virtual void WriteLine(string message, params object[] arguments)
        {
            Console.WriteLine(message, arguments);
        }
    }
}