using OpenTibia.IO;

namespace OpenTibia
{
    public interface IIncomingPacket
    {
        void Read(ByteArrayStreamReader reader);
    }
}