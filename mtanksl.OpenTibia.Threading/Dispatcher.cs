using System;
using System.Collections.Generic;
using System.Threading;

namespace OpenTibia.Threading
{
    public class Dispatcher
    {
        private readonly object sync = new object();

            private bool stop = false;

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
                    if (stop)
                    {
                        break;
                    }

                    if (events.Count == 0)
                    {
                        Monitor.Wait(sync);

                        if (stop)
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

        public void QueueForExecution(Action execute)
        {
            QueueForExecution(new DispatcherEvent(execute) );
        }

        public void QueueForExecution(DispatcherEvent dispatcherEvent)
        {
            lock (sync)
            {
                if ( !stop )
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
                if (stop)
                {
                    wait = false;
                }
                else
                {
                    stop = true;

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