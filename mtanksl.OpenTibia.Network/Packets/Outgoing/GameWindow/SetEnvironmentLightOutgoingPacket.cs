using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SetEnvironmentLightOutgoingPacket : IOutgoingPacket
    {
        public SetEnvironmentLightOutgoingPacket(Light light)
        {
            this.Light = light;
        }

        public Light Light { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x82 );

            writer.Write(Light);
        }
    }
}