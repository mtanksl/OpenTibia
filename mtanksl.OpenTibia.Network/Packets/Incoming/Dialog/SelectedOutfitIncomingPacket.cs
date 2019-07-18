using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class SelectedOutfitIncomingPacket : IIncomingPacket
    {
        public Outfit Outfit { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            Outfit = reader.ReadOutfit();
        }
    }
}