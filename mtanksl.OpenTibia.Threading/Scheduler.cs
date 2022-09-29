using System;
using System.Threading;

namespace OpenTibia.Threading
{
    public class Scheduler
    {
        private readonly object sync = new object();

            private bool stopped = false;

            private AutoResetEvent syncStop = new AutoResetEvent(false);
        
        private Dispatcher dispatcher;

        private Thread worker;
        
        public Scheduler(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;

            this.worker = new Thread(Consume) 
            {
                IsBackground = true, 
                
                Name = "Scheduler thread" 
            };
        }

        public void Start()
        {
            worker.Start();
        }

        private PriorityQueue<SchedulerEvent> events = new PriorityQueue<SchedulerEvent>();

        private void Consume()
        {
            while (true)
            {
                SchedulerEvent schedulerEvent;

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

                    while ( Monitor.Wait(sync, events.Peek().Timeout) )
                    {
                        if (stopped)
                        {
                            break;
                        }
                    }

                    if (stopped)
                    {
                        break;
                    }
                    
                    schedulerEvent = events.Dequeue();
                }

                dispatcher.QueueForExecution(schedulerEvent);
            }

            syncStop.Set();
        }

        public SchedulerEvent QueueForExecution(int executeInMilliseconds, Action execute)
        {
            return QueueForExecution( new SchedulerEvent(executeInMilliseconds, execute) );
        }

        public SchedulerEvent QueueForExecution(SchedulerEvent schedulerEvent)
        {
            lock (sync)
            {
                if ( !stopped )
                {
                    events.Enqueue(schedulerEvent);

                    Monitor.Pulse(sync);
                }
            }

            return schedulerEvent;
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