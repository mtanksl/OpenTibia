using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class ReportBugIncomingPacket : IIncomingPacket
    {
        public string Message { get; set; }
        
        public void Read(IByteArrayStreamReader reader)
        {
            Message = reader.ReadString();
        }
    }
}