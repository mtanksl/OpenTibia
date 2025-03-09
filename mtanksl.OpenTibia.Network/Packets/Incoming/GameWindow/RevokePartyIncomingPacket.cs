using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class RevokePartyIncomingPacket : IIncomingPacket
    {
        public uint CreatureId { get; set; }
        
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            CreatureId = reader.ReadUInt();
        }
    }
}