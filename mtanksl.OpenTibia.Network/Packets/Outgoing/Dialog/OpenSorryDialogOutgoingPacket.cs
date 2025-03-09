using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenSorryDialogOutgoingPacket : IOutgoingPacket
    {
        private bool login;

        public OpenSorryDialogOutgoingPacket(bool login, string message)
        {
            this.login = login;

            this.Message = message;
        }

        public string Message { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            if (login)
            {
                writer.Write( (byte)0x0A );
            }
            else
            {
                writer.Write( (byte)0x14 );
            }

            writer.Write(Message);
        }
    }
}