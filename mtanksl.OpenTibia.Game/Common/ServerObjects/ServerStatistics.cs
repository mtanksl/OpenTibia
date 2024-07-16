using System;

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

        public ulong TotalMessagesSent { get; private set; }

        public ulong TotalBytesSent { get; private set; }

        public ulong TotalMessagesReceived { get; private set; }

        public ulong TotalBytesReceived { get; private set; }

        public void Start()
        {
            startDate = DateTime.UtcNow;
        }

        public void IncreaseMessagesSent()
        {
            TotalMessagesSent += 1;
        }

        public void IncreaseBytesSent(int count)
        {
            TotalBytesSent += (ulong)count;
        }

        public void IncreaseMessagesReceived()
        {
            TotalMessagesReceived += 1;
        }

        public void IncreaseBytesReceived(int count)
        {
            TotalBytesReceived += (ulong)count;
        }
    }
}