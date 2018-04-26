using OpenTibia.IO;
using System.Linq;

namespace OpenTibia.Network.Packets.Incoming
{
    public class UseItem : IIncomingPacket
    {
        public ushort X { get; set; }

        public ushort Y { get; set; }

        public byte Z { get; set; }

        public ushort ItemId { get; set; }

        public byte Index { get; set; }
        
        public byte ContainerId { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            X = reader.ReadUShort();

            Y = reader.ReadUShort();

            Z = reader.ReadByte();

            ItemId = reader.ReadUShort();

            Index = reader.ReadByte();

            ContainerId = reader.ReadByte();
        }

        public void Parse(TibiaClient client)
        {
            /*
            Item item = Position.FromPacket(X, Y, Z, Index).GetContent(client) as Item;
            
            if (item == null || item.Metadata.ClientId != ItemId)
            {
                return;
            }

            if ( !Game.Current.UseItemScripts.Execute(client.Player, item) )
            {
                client.Send(new Text(TextColor.WhiteBottomGameWindow, "You cannot use this object.") );
            }
            */
        }
    }
}