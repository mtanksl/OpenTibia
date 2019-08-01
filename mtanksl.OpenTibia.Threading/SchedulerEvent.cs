using System;

namespace OpenTibia.Threading
{
    public class SchedulerEvent : DispatcherEvent, IComparable<SchedulerEvent>
    {
        public SchedulerEvent(int execution, Action execute) : base(execute)
        {
            this.timeout = DateTime.UtcNow.AddMilliseconds( Math.Max(50, execution) );
        }

        private DateTime timeout;

        public TimeSpan Timeout
        {
            get
            {
                DateTime now = DateTime.UtcNow;

                if (timeout > now)
                {
                    return timeout.Subtract(now);
                }

                return TimeSpan.Zero;
            }
        }

        public int CompareTo(SchedulerEvent scheduledEvent)
        {
            return timeout.CompareTo(scheduledEvent.timeout);
        }
    }
}