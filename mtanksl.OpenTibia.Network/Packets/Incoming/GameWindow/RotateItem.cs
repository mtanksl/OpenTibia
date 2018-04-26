using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class RotateItem : IIncomingPacket
    {
        public ushort X { get; set; }

        public ushort Y { get; set; }

        public byte Z { get; set; }

        public ushort ItemId { get; set; }

        public byte Index { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            X = reader.ReadUShort();

            Y = reader.ReadUShort();

            Z = reader.ReadByte();

            ItemId = reader.ReadUShort();

            Index = reader.ReadByte();
        }
        
        public void Parse(TibiaClient client)
        {
            /*
            Item item = Position.FromPacket(X, Y, Z, Index).GetContent(client) as Item;

            if (item == null || item.Metadata.ClientId != ItemId || !item.Metadata.Flags.Any(ItemMetadataFlags.Rotatable) )
            {
                return;
            }

            if ( !Game.Current.RotateItemScripts.Execute(client.Player, item) )
            {
                client.Send(new Text(TextColor.WhiteBottomGameWindow, "You cannot use this object.") );
            }
            */
        }
    }
}