using OpenTibia.IO;

namespace OpenTibia
{
    public interface IOutgoingPacket
    {
        void Write(ByteArrayStreamWriter writer);
    }
}