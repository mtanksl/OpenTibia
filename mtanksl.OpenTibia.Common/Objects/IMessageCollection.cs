using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IMessageCollection : IDisposable
    {
        void Add(IOutgoingPacket packet, IHasFeatureFlag features);

        IEnumerable<byte[]> GetMessages();
    }
}