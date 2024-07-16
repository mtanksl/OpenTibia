using System;
using System.Threading;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class ServerStatistics : IServerStatistics
    {
        private DateTime? startDate;

        public TimeSpan Uptime
        {
            get
            {
                if (startDate != null)
                {
                    return DateTime.UtcNow - startDate.Value;
                }

                return TimeSpan.Zero;
            }
        }

        private ulong totalMessagesSent;

        public ulong TotalMessagesSent
        { 
            get
            {
                return Interlocked.CompareExchange(ref totalMessagesSent, 0, 0);
            }
        }

        private ulong totalBytesSent;

        public ulong TotalBytesSent
        {
            get
            {
                return Interlocked.CompareExchange(ref totalBytesSent, 0, 0);
            }
        }

        private ulong totalMessagesReceived;

        public ulong TotalMessagesReceived
        {
            get
            {
                return Interlocked.CompareExchange(ref totalMessagesReceived, 0, 0);
            }
        }

        private ulong totalBytesReceived;

        public ulong TotalBytesReceived
        {
            get
            {
                return Interlocked.CompareExchange(ref totalBytesReceived, 0, 0);
            }
        }

        public void Start()
        {
            startDate = DateTime.UtcNow;
        }

        public void IncreaseMessagesSent()
        {
            Interlocked.Increment(ref totalMessagesSent);
        }

        public void IncreaseBytesSent(int count)
        {
            Interlocked.Add(ref totalBytesSent, (ulong)count);
        }

        public void IncreaseMessagesReceived()
        {
            Interlocked.Increment(ref totalMessagesReceived);
        }

        public void IncreaseBytesReceived(int count)
        {
            Interlocked.Add(ref totalBytesReceived, (ulong)count);
        }
    }
}