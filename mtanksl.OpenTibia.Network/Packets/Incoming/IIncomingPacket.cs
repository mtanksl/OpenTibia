using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public interface IIncomingPacket
    {
        void Read(ByteArrayStreamReader reader);
    }
}