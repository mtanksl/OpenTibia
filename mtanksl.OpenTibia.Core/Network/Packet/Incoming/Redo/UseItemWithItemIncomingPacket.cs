using OpenTibia.IO;

namespace OpenTibia
{
    public class UseItemWithItemIncomingPacket : IIncomingPacket
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

        public void Parse(TibiaClient client)
        {
            /*
            Item fromItem = Position.FromPacket(FromX, FromY, FromZ, FromIndex).GetContent(client) as Item;
            
            if (fromItem == null || fromItem.Metadata.ClientId != FromItemId || !fromItem.Metadata.Flags.Any(ItemMetadataFlags.Useable) )
            {
                return;
            }

            Item toItem = Position.FromPacket(ToX, ToY, ToZ, ToIndex).GetContent(client) as Item;
            
            if (toItem == null || toItem.Metadata.ClientId != ToItemId)
            {
                return;
            }

            if ( !Game.Current.UseItemWithItemScripts.Execute(client.Player, fromItem, toItem) )
            {
                client.Send(new TextOutgoingPacket(TextColor.WhiteBottomGameWindow, "You cannot use this object.") );
            }
            */
        }
    }
}