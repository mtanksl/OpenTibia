using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenPleaseWaitDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenPleaseWaitDialogOutgoingPacket(string message, byte time)
        {
            this.Message = message;

            this.Time = time;
        }

        public string Message { get; set; }

        public byte Time { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x16 );

            writer.Write(Message);

            writer.Write(Time);
        }
    }
}