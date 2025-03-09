using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class ProcessReportRuleViolationIncomingPacket : IIncomingPacket
    {
        public string Name { get; set; }
        
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            Name = reader.ReadString();
        }
    }
}