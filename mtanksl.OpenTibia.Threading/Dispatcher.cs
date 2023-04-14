using System.Collections.Generic;
using System.Threading;

namespace OpenTibia.Threading
{
    public class Dispatcher
    {
        private readonly object sync = new object();

            private bool stopped = false;

            private AutoResetEvent syncStop = new AutoResetEvent(false);
        
        private Thread worker;

        public Dispatcher()
        {
            this.worker = new Thread(Consume)
            {
                IsBackground = true, 
                
                Name = "Dispatcher thread" 
            };
        }

        public void Start()
        {
            worker.Start();
        }

        private Queue<DispatcherEvent> events = new Queue<DispatcherEvent>();

        private void Consume()
        {
            while (true)
            {
                DispatcherEvent dispatcherEvent;

                lock (sync)
                {
                    if (stopped)
                    {
                        break;
                    }

                    if (events.Count == 0)
                    {
                        Monitor.Wait(sync);

                        if (stopped)
                        {
                            break;
                        }
                    }

                    dispatcherEvent = events.Dequeue();
                }

                dispatcherEvent.Execute();
            }

            syncStop.Set();
        }

        public void QueueForExecution(DispatcherEvent dispatcherEvent)
        {
            lock (sync)
            {
                if ( !stopped )
                {
                    events.Enqueue(dispatcherEvent);

                    Monitor.Pulse(sync);
                }
            }
        }

        public void Stop(bool wait = true)
        {
            lock (sync)
            {
                if (stopped)
                {
                    wait = false;
                }
                else
                {
                    stopped = true;

                    Monitor.Pulse(sync);
                }
            }

            if (wait)
            {
                syncStop.WaitOne();
            }
        }
    }
}