﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class StopWalkOutgoingPacket : IOutgoingPacket
    {
        public StopWalkOutgoingPacket(Direction direction)
        {
            this.Direction = direction;
        }

        public Direction Direction { get; set; }
                
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xB5 );

            writer.Write( (byte)Direction );
        }
    }
}