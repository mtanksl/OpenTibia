using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class UseItemWithItem : IIncomingPacket
    {
        public ushort FromX { get; set; }

        public ushort FromY { get; set; }

        public byte FromZ { get; set; }

        public ushort FromItemId { get; set; }

        public byte FromIndex { get; set; }

        public ushort ToX { get; set; }

        public ushort ToY { get; set; }

        public byte ToZ { get; set; }

        public ushort ToItemId { get; set; }

        public byte ToIndex { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            FromX = reader.ReadUShort();

            FromY = reader.ReadUShort();

            FromZ = reader.ReadByte();

            FromItemId = reader.ReadUShort();

            FromIndex = reader.ReadByte();

            ToX = reader.ReadUShort();

            ToY = reader.ReadUShort();

            ToZ = reader.ReadByte();

            ToItemId = reader.ReadUShort();

            ToIndex = reader.ReadByte();
        }
    }
}