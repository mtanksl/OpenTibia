using System;
using System.Collections.Generic;
using System.Threading;

namespace OpenTibia.Threading
{
    public class Dispatcher : SynchronizationContext
    {
        private readonly object sync = new object();

            private bool stop = false;

            private AutoResetEvent syncStop = new AutoResetEvent(false);

            private Queue<DispatcherEvent> events = new Queue<DispatcherEvent>();

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

        private void Consume()
        {
            SynchronizationContext.SetSynchronizationContext(this);

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

                using (DispatcherContext context = new DispatcherContext() )
                {
                    dispatcherEvent.Execute();

                    OnComplete();
                }
            }

            syncStop.Set();
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            QueueForExecution( () =>
            {
                d(state);
            } );
        }

        public void QueueForExecution(Action execute)
        {
            QueueForExecution( new DispatcherEvent(execute) );
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

        public event EventHandler Complete;

        protected virtual void OnComplete()
        {
            if (Complete != null)
            {
                Complete(this, EventArgs.Empty);
            }
        }
    }
}