using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SetLight : IOutgoingPacket
    {
        public SetLight(uint creatureId, Light light)
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