﻿using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class LookItemNpcTradeIncomingPacket : IIncomingPacket
    {
        public ushort TibiaId { get; set; }

        public byte Type { get; set; }
        
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            TibiaId = reader.ReadUShort();

            Type = reader.ReadByte();
        }
    }
}