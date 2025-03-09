using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class LookItemTradeIncomingPacket : IIncomingPacket
    {
        public byte WindowId { get; set; }

        public byte Index { get; set; }
        
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            WindowId = reader.ReadByte();

            Index = reader.ReadByte();
        }
    }
}