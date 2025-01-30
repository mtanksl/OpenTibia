using OpenTibia.IO;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Network.Packets.Incoming
{
    public class MiscInfoOutgoingPacket : IOutgoingPacket
    {
        public MiscInfoOutgoingPacket(string motd, string location, string url, uint uptime)
        {
            Motd = motd;

            Location = location;

            Url = url;

            Uptime = uptime;
        }

        public string Motd { get; set; }

        public string Location { get; set; }

        public string Url { get; set; }

        public uint Uptime { get; set; }

        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x12);

            writer.Write(Motd);

            writer.Write(Location);

            writer.Write(Url);

            writer.Write(Uptime);
        }
    }
}