using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Tests
{
    public class MockMessageCollection : IMessageCollection
    {
        private List<IOutgoingPacket> outgoingPackets = new List<IOutgoingPacket>();

        public IEnumerable<IOutgoingPacket> OutgoingPackets
        {
            get
            {
                return outgoingPackets;
            }
        }

        public void Add(IOutgoingPacket packet, IHasFeatureFlag features)
        {
            outgoingPackets.Add(packet);
        }

        public IEnumerable<byte[]> GetMessages()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }
    }
}