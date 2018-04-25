using OpenTibia.IO;

namespace OpenTibia
{
    public class ExcludePlayerIncomingPacket : IIncomingPacket
    {
        public string Name { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            Name = reader.ReadString();
        }
    }
}