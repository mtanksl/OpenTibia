using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia
{
    public class InviteTradeOutgoingPacket : IOutgoingPacket
    {
        public InviteTradeOutgoingPacket(string name, List<Item> items)
        {
            this.Name = name;

            this.Items = items;
        }

        public string Name { get; set; }

        private List<Item> items;

        public List<Item> Items
        {
            get
            {
                return items ?? ( items = new List<Item>() );
            }
            set
            {
                items = value;
            }
        }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x7D );

            writer.Write(Name);

            writer.Write( (byte)Items.Count );

            foreach (var item in items)
            {
                writer.Write(item);
            }
        }
    }
}