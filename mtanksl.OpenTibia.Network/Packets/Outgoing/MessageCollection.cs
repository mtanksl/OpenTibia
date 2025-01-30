using OpenTibia.Common.Objects;
using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class MessageCollection : IMessageCollection
    {
        private const int MaxPacketSize = 8192;

        private const int MaxMessageSize = 15000; //TODO: What is the max message size?

        private static byte[] buffer;

        private static ByteArrayArrayStream bufferStream;

        private static ByteArrayStreamWriter bufferWriter;

        static MessageCollection()
        {
            buffer = new byte[MaxPacketSize];

            bufferStream = new ByteArrayArrayStream(buffer);

            bufferWriter = new ByteArrayStreamWriter(bufferStream);
        }

        private LinkedList<Message> messages;

        private Message message = new Message();

        public void Add(IOutgoingPacket packet)
        {
            bufferStream.Seek(Origin.Begin, 0);

            packet.Write(bufferWriter);

            if (message.Position + bufferStream.Position > MaxMessageSize)
            {
                if (messages == null)
                {
                    messages = new LinkedList<Message>();
                }

                messages.AddLast(message);

                message = new Message();
            }

            message.Write(buffer, 0, bufferStream.Position);
        }

        public IEnumerable<IMessage> GetMessages()
        {
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    yield return message;
                }
            }

            yield return message;
        }
    }
}