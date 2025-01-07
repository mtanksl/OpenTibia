using System;
using System.Diagnostics;
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

        private uint playersPeek;

        public uint PlayersPeek
        {
            get
            {
                return playersPeek;
            }
            set
            {
                playersPeek = value;
            }
        }

        private ulong activeConnections;

        public ulong ActiveConnections
        {
            get
            {
                return Interlocked.CompareExchange(ref activeConnections, 0, 0);
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

        private double averageProcessingTime;

        public double AverageProcessingTime
        {
            get
            {
                return Interlocked.CompareExchange(ref averageProcessingTime, 0, 0);
            }
        }

        public void Start()
        {
            startDate = DateTime.UtcNow;
        }

        public void IncreaseActiveConnection()
        {
            Interlocked.Increment(ref activeConnections);
        }

        public void DecreaseActiveConnection()
        {
            Interlocked.Decrement(ref activeConnections);
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

        private const int sampleLength = 100;

        private long[] samples = new long[sampleLength];

        private int sampleIndex = 0;

        private long sampleSum = 0;

        public void IncreaseProcessingTime(long elapsedTicks)
        {
            sampleSum -= samples[sampleIndex];

            sampleSum += samples[sampleIndex] = elapsedTicks;

            sampleIndex = ++sampleIndex % sampleLength;

            Interlocked.Exchange(ref averageProcessingTime, (double)sampleSum / sampleLength / Stopwatch.Frequency * 1000);
        }
    }
}