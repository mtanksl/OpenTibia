using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class SharedExperienceIncomingPacket : IIncomingPacket
    {
        public bool Enabled { get; set; }
        
        public void Read(IByteArrayStreamReader reader)
        {
            Enabled = reader.ReadBool();
        }
    }
}