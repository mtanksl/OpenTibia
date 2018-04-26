using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class SharedExperience : IIncomingPacket
    {
        public bool Enabled { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            Enabled = reader.ReadBool();
        }
    }
}