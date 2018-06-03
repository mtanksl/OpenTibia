using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class CancelRuleViolation : IOutgoingPacket
    {
        public CancelRuleViolation(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xB0 );

            writer.Write(Name);
        }
    }
}