using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class CloseContainerIncommingPacket : IIncomingPacket
    {
        public byte ContainerId { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            ContainerId = reader.ReadByte();
        }
    }
}