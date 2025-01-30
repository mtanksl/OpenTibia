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
        
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xA2 );

            writer.Write( (ushort)SpecialCondition );
        }
    }
}