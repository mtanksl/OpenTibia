using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Tests
{
    public class Assertion
    {
        public Type Type { get; set; }

        public int Total { get; set; }

        public Func<IOutgoingPacket, bool> Callback { get; set; }
    }
}