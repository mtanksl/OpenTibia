using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class RevokeParty : IIncomingPacket
    {
        public uint CreatureId { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            CreatureId = reader.ReadUInt();
        }
    }
}