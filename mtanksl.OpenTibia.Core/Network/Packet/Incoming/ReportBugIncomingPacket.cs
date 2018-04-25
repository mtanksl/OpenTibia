using OpenTibia.IO;

namespace OpenTibia
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