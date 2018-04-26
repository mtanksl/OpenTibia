using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class MoveItem : IIncomingPacket
    {
        public ushort FromX { get; set; }

        public ushort FromY { get; set; }

        public byte FromZ { get; set; }

        public ushort FromItemId { get; set; }

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

            FromItemId = reader.ReadUShort();

            FromIndex = reader.ReadByte();

            ToX = reader.ReadUShort();

            ToY = reader.ReadUShort();

            ToZ = reader.ReadByte();

            Count = reader.ReadByte();
        }
        
        public void Parse(TibiaClient client)
        {
            /*
            Position fromPosition = Position.FromPacket(FromX, FromY, FromZ, FromIndex);

            Position toPosition = Position.FromPacket(ToX, ToY, ToZ);
            
                Item item = fromPosition.GetContent(client) as Item;

                if (item == null || item.Metadata.ClientId != FromItemId || item.Metadata.Flags.Any(ItemMetadataFlags.NotMoveable) )
                {
                    return;
                }

            fromPosition.Remove(client);

            toPosition.Add(client, item);
            */
        }
    }
}