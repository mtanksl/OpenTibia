using OpenTibia.IO;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Network.Packets.Incoming
{
    public class SoftwareInfoOutgoingPacket : IOutgoingPacket
    {
        public SoftwareInfoOutgoingPacket(string serverName, string serverVersion, string clientVersion)
        {
            ServerName = serverName;

            ServerVersion = serverVersion;

            ClientVersion = clientVersion;
        }

        public string ServerName { get; set; }

        public string ServerVersion { get; set; }

        public string ClientVersion { get; set; }

        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x23);

            writer.Write(ServerName);

            writer.Write(ServerVersion);

            writer.Write(ClientVersion);
        }
    }
}