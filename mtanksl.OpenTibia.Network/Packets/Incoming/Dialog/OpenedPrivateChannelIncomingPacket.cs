using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class OpenedPrivateChannelIncomingPacket : IIncomingPacket
    {
        public string Name { get; set; }
        
        public void Read(IByteArrayStreamReader reader)
        {
            Name = reader.ReadString();
        }
    }
}