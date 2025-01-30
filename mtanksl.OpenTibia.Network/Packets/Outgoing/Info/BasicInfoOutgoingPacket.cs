using OpenTibia.IO;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Network.Packets.Incoming
{
    public class BasicInfoOutgoingPacket : IOutgoingPacket
    {
        public BasicInfoOutgoingPacket(string serverName, string ipAddress, int loginPort)
        {
            ServerName = serverName;

            IPAddress = ipAddress;

            LoginPort = loginPort;
        }

        public string ServerName { get; set; }

        public string IPAddress { get; set; }

        public int LoginPort { get; set; }

        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x10);

            writer.Write(ServerName);

            writer.Write(IPAddress);

            writer.Write(LoginPort.ToString() );
        }
    }
}