using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class MoveItemIncomingPacket : IIncomingPacket
    {
        public ushort FromX { get; set; }

        public ushort FromY { get; set; }

        public byte FromZ { get; set; }

        public ushort FromTibiaId { get; set; }

        public byte FromIndex { get; set; }

        public ushort ToX { get; set; }

        public ushort ToY { get; set; }

        public byte ToZ { get; set; }

        public byte Count { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            FromX = reader.ReadUShort();

            FromY = reader.ReadUShort();

            FromZ = reader.ReadByte();

            FromTibiaId = reader.ReadUShort();

            FromIndex = reader.ReadByte();

            ToX = reader.ReadUShort();

            ToY = reader.ReadUShort();

            ToZ = reader.ReadByte();

            Count = reader.ReadByte();
        }
    }
}