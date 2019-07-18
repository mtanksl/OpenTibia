using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class ReportBugIncomingPacket : IIncomingPacket
    {
        public string Message { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            Message = reader.ReadString();
        }
    }
}