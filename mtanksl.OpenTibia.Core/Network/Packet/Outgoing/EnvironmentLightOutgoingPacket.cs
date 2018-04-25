using OpenTibia.IO;

namespace OpenTibia
{
    public class EnvironmentLightOutgoingPacket : IOutgoingPacket
    {
        public EnvironmentLightOutgoingPacket(Light light)
        {
            this.Light = light;
        }

        public Light Light { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x82 );

            writer.Write(Light);
        }
    }
}