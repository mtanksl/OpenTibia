using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IServerStatistics
    {
        TimeSpan Uptime { get; }

        ulong ActiveConnections { get; }

        ulong TotalMessagesSent { get; }

        ulong TotalBytesSent { get; }

        ulong TotalMessagesReceived { get; }

        ulong TotalBytesReceived { get; }

        double AverageProcessingTime { get; }

        void Start();

        void IncreaseActiveConnection();

        void DecreaseActiveConnection();

        void IncreaseMessagesSent();

        void IncreaseBytesSent(int count);

        void IncreaseMessagesReceived();

        void IncreaseBytesReceived(int count);

        void IncreaseProcessingTime(long elapsedTicks);
    }
}