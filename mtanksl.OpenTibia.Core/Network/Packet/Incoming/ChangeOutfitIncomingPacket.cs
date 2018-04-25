using OpenTibia.IO;

namespace OpenTibia
{
    public class ChangeOutfitIncomingPacket : IIncomingPacket
    {
        public Outfit Outfit { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            Outfit = reader.ReadOutfit();
        }
    }
}