using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class DebugAssertIncomingPacket : IIncomingPacket
    {
        public string AssertLine { get; set; }

        public string ReportDate { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        public void Read(IByteArrayStreamReader reader)
        {
            AssertLine = reader.ReadString();

            ReportDate = reader.ReadString();

            Description = reader.ReadString();

            Comment = reader.ReadString();
        }
    }
}