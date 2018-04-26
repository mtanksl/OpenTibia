using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class UseItemWithCreature : IIncomingPacket
    {
        public ushort X { get; set; }

        public ushort Y { get; set; }

        public byte Z { get; set; }

        public ushort ItemId { get; set; }

        public byte Index { get; set; }

        public uint CreatureId { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            X = reader.ReadUShort();

            Y = reader.ReadUShort();

            Z = reader.ReadByte();

            ItemId = reader.ReadUShort();

            Index = reader.ReadByte();

            CreatureId = reader.ReadUInt();
        }
        
        public void Parse(TibiaClient client)
        {
            /*
            Item fromItem = Position.FromPacket(X, Y, Z, Index).GetContent(client) as Item;
            
            if (fromItem == null || fromItem.Metadata.ClientId != ItemId || !fromItem.Metadata.Flags.Any(ItemMetadataFlags.Useable) )
            {
                return;
            }

            Creature toCreature = Game.Current.Map.GetCreature(CreatureId);

            if (toCreature == null)
            {
                return;
            }

            if ( !Game.Current.UseItemWithCreatureScripts.Execute(client.Player, fromItem, toCreature) )
            {
                client.Send(new Text(TextColor.WhiteBottomGameWindow, "You cannot use this object.") );
            }
            */
        }
    }
}