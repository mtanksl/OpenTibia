using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class OpenReportRuleViolationChannel : IIncomingPacket
    {
        public string Name { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            Name = reader.ReadString();
        }
    }
}