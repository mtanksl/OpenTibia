using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class CloseChannelIncomingPacket : IIncomingPacket
    {
        public ushort ChannelId { get; set; }
        
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            ChannelId = reader.ReadUShort();
        }
    }
}