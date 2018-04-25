using OpenTibia.IO;

namespace OpenTibia
{
    public class LightOutgoingPacket : IOutgoingPacket
    {
        public LightOutgoingPacket(uint creatureId, Light light)
        {
            this.CreatureId = creatureId;

            this.Light = light;
        }

        public uint CreatureId { get; set; }

        public Light Light { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x8D );

            writer.Write(CreatureId);

            writer.Write(Light);
        }
    }
}