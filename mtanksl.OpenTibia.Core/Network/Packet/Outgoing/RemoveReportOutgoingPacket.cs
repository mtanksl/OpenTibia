using OpenTibia.IO;

namespace OpenTibia
{
    public class RemoveReportOutgoingPacket : IOutgoingPacket
    {
        public RemoveReportOutgoingPacket(string name)
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