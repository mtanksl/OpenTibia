using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IServerStatistics
    {
        TimeSpan Uptime { get; }

        ulong TotalMessagesSent { get; }

        ulong TotalBytesSent { get; }

        ulong TotalMessagesReceived { get; }

        ulong TotalBytesReceived { get; }

        void Start();

        void IncreaseMessagesSent();

        void IncreaseBytesSent(int count);

        void IncreaseMessagesReceived();

        void IncreaseBytesReceived(int count);
    }
}