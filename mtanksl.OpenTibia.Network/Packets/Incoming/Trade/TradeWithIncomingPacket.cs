using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class TradeWithIncomingPacket : IIncomingPacket
    {
        public ushort X { get; set; }

        public ushort Y { get; set; }

        public byte Z { get; set; }

        public ushort TibiaId { get; set; }

        public byte Index { get; set; }

        public uint CreatureId { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            X = reader.ReadUShort();

            Y = reader.ReadUShort();

            Z = reader.ReadByte();

            TibiaId = reader.ReadUShort();

            Index = reader.ReadByte();

            CreatureId = reader.ReadUInt();
        }
    }
}