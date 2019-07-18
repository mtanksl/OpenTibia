using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class AddVipIncommingPacket : IIncomingPacket
    {
        public string Name { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            Name = reader.ReadString();
        }
    } 
}