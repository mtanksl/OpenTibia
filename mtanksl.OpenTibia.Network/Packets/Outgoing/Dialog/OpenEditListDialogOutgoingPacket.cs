using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenEditListDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenEditListDialogOutgoingPacket(uint windowId, string text)
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