using System;

namespace OpenTibia.Threading
{
    public class SchedulerEvent : DispatcherEvent, IComparable<SchedulerEvent>
    {
        private DateTime executeIn;

        public SchedulerEvent(int execution, Action execute) : base(execute)
        {
            this.executeIn = DateTime.UtcNow.AddMilliseconds( Math.Max(50, execution) );
        }

        public TimeSpan Timeout
        {
            get
            {
                DateTime now = DateTime.UtcNow;

                return executeIn > now ? executeIn.Subtract(now) : TimeSpan.Zero;
            }
        }

        public int CompareTo(SchedulerEvent scheduledEvent)
        {
            return executeIn.CompareTo(scheduledEvent.executeIn);
        }
    }
}