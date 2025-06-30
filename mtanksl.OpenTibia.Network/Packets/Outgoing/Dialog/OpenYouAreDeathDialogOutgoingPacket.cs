using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenYouAreDeathDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenYouAreDeathDialogOutgoingPacket(DeathType deathType, byte penality)
        {
            this.Penality = penality;

            this.DeathType = deathType;
        }

        public DeathType DeathType { get; set; }

        public byte Penality { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x28 );

            if (features.HasFeatureFlag(FeatureFlag.DeathType) )
            {
                writer.Write( (byte)DeathType);
            }

            if (features.HasFeatureFlag(FeatureFlag.PenalityOnDeath) && DeathType == DeathType.Regular)
            {
                writer.Write(Penality);
            }
        }
    }
}