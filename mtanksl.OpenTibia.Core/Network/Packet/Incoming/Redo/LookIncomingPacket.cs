using OpenTibia.IO;

namespace OpenTibia
{
    public class LookIncomingPacket : IIncomingPacket
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
            IContent content = Position.FromPacket(X, Y, Z, Index).GetContent(client);

            if (content == null)
            {
                return;
            }

            StringBuilder message = new StringBuilder("You see ");

            if (content is Item)
	        {
                Item item = (Item)content;

                if (item.Metadata.ClientId != ItemId)
                {
                    return;
                }

                if (item is Stackable)
                {
                    Stackable stackable = (Stackable)item;

                    message.Append(stackable.Count).Append(" ").Append(item.Metadata.Plural).Append("."); 
                }
                else
                {
                    message.Append(item.Metadata.Article).Append(" ").Append(item.Metadata.Name).Append(".");
                }
	        }
            else if (content is Creature)
	        {
		        Creature creature = (Creature)content;

                if (0x63 != ItemId)
                {
                    return;
                }
                
                message.Append(creature.Name).Append(".");
	        }

            client.Send(new TextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, message.ToString() ) );
            */
        }
    }
}