using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SetSpeedOutgoingPacket : IOutgoingPacket
    {
        public SetSpeedOutgoingPacket(uint creatureId, ushort speed)
        {
            this.CreatureId = creatureId;

            this.Speed = speed;
        }

        public uint CreatureId { get; set; }
        
        public ushort Speed { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x8F );

            writer.Write(CreatureId);

            if ( !features.HasFeatureFlag(FeatureFlag.NewSpeedLaw) )
            {
                writer.Write(Speed);
            }
            else
            {
                writer.Write( (ushort)(Speed / 2) );
            }
        }
    }
}