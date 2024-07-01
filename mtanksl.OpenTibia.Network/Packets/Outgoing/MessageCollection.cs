using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class MessageCollection
    {
        private const int MaxPacketSize = 8192;

        private const int MaxMessageSize = 15000;

        private static readonly object sync = new object();

        private static byte[] tempArray;

        private static ByteArrayArrayStream tempStream;

        private static ByteArrayStreamWriter tempWriter;

        static MessageCollection()
        {
            tempArray = new byte[MaxPacketSize];

            tempStream = new ByteArrayArrayStream(tempArray);

            tempWriter = new ByteArrayStreamWriter(tempStream);
        }

        private LinkedList<Message> messages;

        private Message current = new Message();

        public void Add(IOutgoingPacket packet)
        {
            lock (sync)
            {
                tempStream.Seek(Origin.Begin, 0);

                packet.Write(tempWriter);

                if (current.Position + tempStream.Position > MaxMessageSize)
                {
                    if (messages == null)
                    {
                        messages = new LinkedList<Message>();
                    }

                    messages.AddLast(current);

                    current = new Message();
                }

                current.Write(tempArray, 0, tempStream.Position);
            }
        }

        public IEnumerable<Message> GetMessages()
        {
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    yield return message;
                }
            }

            yield return current;
        }
    }
}