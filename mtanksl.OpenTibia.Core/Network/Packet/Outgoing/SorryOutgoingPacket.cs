using OpenTibia.IO;

namespace OpenTibia
{
    public class SorryOutgoingPacket : IOutgoingPacket
    {
        private bool login;

        public SorryOutgoingPacket(bool login, string message)
        {
            this.login = login;

            this.Message = message;
        }

        public string Message { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
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