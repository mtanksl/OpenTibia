using OpenTibia.IO;

namespace OpenTibia
{
    public class EditListOutgoingPacket : IOutgoingPacket
    {
        public EditListOutgoingPacket(uint windowId, string text)
        {
            this.WindowId = windowId;

            this.Text = text;
        }

        public uint WindowId { get; set; }
              
        public string Text { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x97 );

            writer.Write( (byte)0x00 );

            writer.Write(WindowId);

            writer.Write(Text);
        }
    }
}