using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class CloseContainerIncomingPacket : IIncomingPacket
    {
        public byte ContainerId { get; set; }
        
        public void Read(IByteArrayStreamReader reader)
        {
            ContainerId = reader.ReadByte();
        }
    }
}