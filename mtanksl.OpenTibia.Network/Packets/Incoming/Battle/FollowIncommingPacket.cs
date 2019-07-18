using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class FollowIncommingPacket : IIncomingPacket
    {
        public uint CreatureId { get; set; }

        public uint Nonce { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            CreatureId = reader.ReadUInt();

            Nonce = reader.ReadUInt();
        }
    }
}