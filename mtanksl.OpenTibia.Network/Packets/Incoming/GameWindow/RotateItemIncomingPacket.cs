using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class RotateItemIncomingPacket : IIncomingPacket
    {
        public ushort X { get; set; }

        public ushort Y { get; set; }

        public byte Z { get; set; }

        public ushort TibiaId { get; set; }

        public byte Index { get; set; }
        
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            X = reader.ReadUShort();

            Y = reader.ReadUShort();

            Z = reader.ReadByte();

            TibiaId = reader.ReadUShort();

            Index = reader.ReadByte();
        }
    }
}