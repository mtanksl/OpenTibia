using OpenTibia.IO;

namespace OpenTibia
{
    public class CancelReportOutgoingPacket : IOutgoingPacket
    {
        public CancelReportOutgoingPacket(string name)
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