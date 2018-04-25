using OpenTibia.IO;

namespace OpenTibia
{
    public class ForYourInformationOutgoingPacket : IOutgoingPacket
    {
        private bool login;

        public ForYourInformationOutgoingPacket(bool login, string message)
        {
            this.login = login;

            this.Message = message;
        }

        public string Message { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
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