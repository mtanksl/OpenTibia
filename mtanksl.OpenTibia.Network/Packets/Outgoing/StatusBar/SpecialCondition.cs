using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SpecialCondition : IOutgoingPacket
    {
        public SpecialCondition(SpecialCondition specialCondition)
        {
            this.SpecialCondition = specialCondition;
        }

        public SpecialCondition SpecialCondition { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xA2 );

            writer.Write( (ushort)SpecialCondition );
        }
    }
}