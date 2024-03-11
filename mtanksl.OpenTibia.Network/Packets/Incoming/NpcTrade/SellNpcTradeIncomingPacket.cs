﻿using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class SellNpcTradeIncomingPacket : IIncomingPacket
    {
        public ushort TibiaId { get; set; }

        public byte Type { get; set; }

        public byte Count { get; set; }

        public bool KeepEquipped { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            TibiaId = reader.ReadUShort();

            Type = reader.ReadByte();

            Count = reader.ReadByte();

            KeepEquipped = reader.ReadBool();
        }
    }
}