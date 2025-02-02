using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class Message : IMessage
    {
        private ByteArrayMemoryStream stream;

        private ByteArrayStreamWriter writer;

        public Message()
        {
            stream = new ByteArrayMemoryStream();

            writer = new ByteArrayStreamWriter(stream);
        }

        public int Position
        {
            get
            {
                return stream.Position;
            }
        }

        public void Write(byte[] buffer, int offset, int length)
        {
            writer.Write(buffer, offset, length);
        }

        public byte[] GetBytes()
        {
            return stream.GetBytes();
        }
    }
}