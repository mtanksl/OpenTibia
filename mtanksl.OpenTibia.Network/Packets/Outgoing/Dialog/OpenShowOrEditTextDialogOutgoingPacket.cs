using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenShowOrEditTextDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenShowOrEditTextDialogOutgoingPacket(uint windowId, ushort tibiaId, ushort maxLength, string text, string author, string date)
        {
            this.WindowId = windowId;

            this.TibiaId = tibiaId;

            this.MaxLength = maxLength;

            this.Text = text;

            this.Author = author;

            this.Date = date;
        }

        public uint WindowId { get; set; }

        public ushort TibiaId { get; set; }

        public ushort MaxLength { get; set; }

        public string Text { get; set; }
        
        public string Author { get; set; }

        public string Date { get; set; }
        
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x96 );

            writer.Write(WindowId);

            writer.Write(TibiaId);

            writer.Write(MaxLength);

            writer.Write(Text);

            writer.Write(Author);

            writer.Write(Date);
        }
    }
}