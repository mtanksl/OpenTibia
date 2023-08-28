using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenEditListDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenEditListDialogOutgoingPacket(byte doorId, uint windowId, string text)
        {
            this.DoorId = doorId;

            this.WindowId = windowId;

            this.Text = text;
        }

        public byte DoorId { get; set; }

        public uint WindowId { get; set; }
              
        public string Text { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x97 );

            writer.Write(DoorId);

            writer.Write(WindowId);

            writer.Write(Text);
        }
    }
}