using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenTextDialog : IOutgoingPacket
    {
        public OpenTextDialog(uint windowId, ushort itemId, ushort maxLength, string text, string author, string date)
        {
            this.WindowId = windowId;

            this.ItemId = itemId;

            this.MaxLength = maxLength;

            this.Text = text;

            this.Author = author;

            this.Date = date;
        }

        public uint WindowId { get; set; }

        public ushort ItemId { get; set; }

        public ushort MaxLength { get; set; }

        public string Text { get; set; }
        
        public string Author { get; set; }

        public string Date { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x96 );

            writer.Write(WindowId);

            writer.Write(ItemId);

            writer.Write(MaxLength);

            writer.Write(Text);

            writer.Write(Author);

            writer.Write(Date);
        }
    }
}