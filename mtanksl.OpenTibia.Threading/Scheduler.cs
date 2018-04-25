using System;
using System.Threading;

namespace OpenTibia.Threading
{
    public class Scheduler
    {
        private readonly object sync = new object();

            private bool stop = false;

            private AutoResetEvent syncStop = new AutoResetEvent(false);

            private PriorityQueue<SchedulerEvent> events = new PriorityQueue<SchedulerEvent>();

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

        private void Consume()
        {
            while (true)
            {
                SchedulerEvent schedulerEvent;

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

                    bool stop2 = false;

                    while ( Monitor.Wait(sync, events.Peek().Timeout) )
                    {
                        if (stop)
                        {
                            stop2 = true;

                            break;
                        }
                    }

                    if (stop2)
                    {
                        break;
                    }
                    
                    schedulerEvent = events.Dequeue();
                }

                dispatcher.QueueForExecution(schedulerEvent);
            }

            syncStop.Set();
        }

        public SchedulerEvent QueueForExecution(int executeIn, Action execute)
        {
            return QueueForExecution( new SchedulerEvent(executeIn, execute) );
        }

        public SchedulerEvent QueueForExecution(SchedulerEvent schedulerEvent)
        {
            lock (sync)
            {
                if ( !stop )
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