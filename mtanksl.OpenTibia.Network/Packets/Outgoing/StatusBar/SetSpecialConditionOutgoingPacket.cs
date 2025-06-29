using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SetSpecialConditionOutgoingPacket : IOutgoingPacket
    {
        public SetSpecialConditionOutgoingPacket(SpecialCondition specialCondition)
        {
            this.SpecialCondition = specialCondition;
        }

        public SpecialCondition SpecialCondition { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xA2 );

            if ( !features.HasFeatureFlag(FeatureFlag.PlayerSpecialConditionUInt16) )
            {
                writer.Write( (byte)( (int)SpecialCondition & 0xFF) );
            }
            else
            {
                writer.Write( (ushort)( (int)SpecialCondition & 0xFFFF) );
            }
        }
    }
}