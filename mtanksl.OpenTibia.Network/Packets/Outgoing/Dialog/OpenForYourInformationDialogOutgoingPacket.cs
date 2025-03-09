using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenForYourInformationDialogOutgoingPacket : IOutgoingPacket
    {
        private bool login;

        public OpenForYourInformationDialogOutgoingPacket(bool login, string message)
        {
            this.login = login;

            this.Message = message;
        }

        public string Message { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            if (login)
            {
                writer.Write( (byte)0x0B );
            }
            else
            {
                writer.Write( (byte)0x15 );                
            }

            writer.Write(Message);
        }
    }
}