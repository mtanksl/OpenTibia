using OpenTibia.IO;

namespace OpenTibia
{
    public class CachedOutgoingPacket : IOutgoingPacket
    {
        private byte[] bytes;

        public CachedOutgoingPacket(IOutgoingPacket packet)
        {
            ByteArrayMemoryStream stream = new ByteArrayMemoryStream();

            ByteArrayStreamWriter writer = new ByteArrayStreamWriter(stream);

            packet.Write(writer);

            this.bytes = stream.GetBytes();
        }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write(bytes);
        }
    }
}