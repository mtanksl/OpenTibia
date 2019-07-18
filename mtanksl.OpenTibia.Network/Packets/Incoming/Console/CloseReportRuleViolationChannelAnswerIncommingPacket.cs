using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class CloseReportRuleViolationChannelAnswerIncommingPacket : IIncomingPacket
    {
        public string Name { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            Name = reader.ReadString();
        }
    }
}