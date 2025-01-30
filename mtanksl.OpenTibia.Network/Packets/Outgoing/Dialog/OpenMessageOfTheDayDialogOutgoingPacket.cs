using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenMessageOfTheDayDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenMessageOfTheDayDialogOutgoingPacket(int number, string message)
        {
            this.Number = number;

            this.Message = message;
        }

        public int Number { get; set; }

        public string Message { get; set; }
        
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x14 );

            writer.Write(Number + "\n" + Message);
        }
    }
}