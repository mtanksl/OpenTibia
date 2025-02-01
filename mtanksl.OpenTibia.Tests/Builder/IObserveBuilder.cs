using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Tests
{
    public interface IObserveBuilder
    {
        IConnection Connection { get; }

        IObserveBuilder ExpectPacket(int total);

        IObserveBuilder ExpectPacket<TPacket>(int total) where TPacket : IOutgoingPacket;

        IObserveBuilder ExpectPacket<TPacket>(int total, Func<TPacket, bool> callback) where TPacket : IOutgoingPacket;

        IObserveBuilder ExpectConnected(bool connected = true);
    }
}