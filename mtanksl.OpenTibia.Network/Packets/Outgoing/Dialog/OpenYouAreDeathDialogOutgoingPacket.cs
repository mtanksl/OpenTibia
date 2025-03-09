using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenYouAreDeathDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenYouAreDeathDialogOutgoingPacket(byte penality)
        {
            this.Penality = penality;
        }

        public byte Penality { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x28 );

            if (features.HasFeatureFlag(FeatureFlag.PenalityOnDeath) )
            {
                writer.Write(Penality);
            }
        }
    }
}