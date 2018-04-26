using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class Sorry : IOutgoingPacket
    {
        private bool login;

        public Sorry(bool login, string message)
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