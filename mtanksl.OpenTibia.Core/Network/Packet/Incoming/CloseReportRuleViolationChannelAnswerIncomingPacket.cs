using OpenTibia.IO;

namespace OpenTibia
{
    public class CloseReportRuleViolationChannelAnswerIncomingPacket : IIncomingPacket
    {
        public string Name { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            Name = reader.ReadString();
        }
    }
}