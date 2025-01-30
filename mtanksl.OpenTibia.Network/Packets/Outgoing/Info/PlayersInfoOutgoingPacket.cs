using OpenTibia.IO;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Network.Packets.Incoming
{
    public class PlayersInfoOutgoingPacket : IOutgoingPacket
    {
        public PlayersInfoOutgoingPacket(uint online, uint max, uint peek)
        {
            Online = online;

            Max = max;

            Peek = peek;
        }

        public uint Online { get; set; }

        public uint Max { get; set; }

        public uint Peek { get; set; }

        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x20);

            writer.Write(Online);

            writer.Write(Max);

            writer.Write(Peek);
        }
    }
}