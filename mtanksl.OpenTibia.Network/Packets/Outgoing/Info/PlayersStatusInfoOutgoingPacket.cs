using OpenTibia.Common.Objects;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Network.Packets.Incoming
{
    public class PlayersStatusInfoOutgoingPacket : IOutgoingPacket
    {
        public PlayersStatusInfoOutgoingPacket(bool online)
        {
            Online = online;
        }

        public bool Online { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x22);

            writer.Write(Online);
        }
    }
}