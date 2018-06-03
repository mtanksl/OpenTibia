using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class RemoveRuleViolation : IOutgoingPacket
    {
        public RemoveRuleViolation(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xAF );

            writer.Write(Name);
        }
    }
}