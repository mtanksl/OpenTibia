﻿using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class RevokePartyIncomingPacket : IIncomingPacket
    {
        public uint CreatureId { get; set; }
        
        public void Read(IByteArrayStreamReader reader)
        {
            CreatureId = reader.ReadUInt();
        }
    }
}