using OpenTibia.Common.Objects;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Outgoing;
using System.Buffers;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class MessageCollection : IMessageCollection
    {
        private const int MaxPacketSize = 8192;

        private const int MaxMessageSize = 16384 - 2 /* length */ - 8 /* padding */ - 4 /* hash */ - 2 /* length*/;

        private static byte[] temp;

        private static ByteArrayArrayStream tempStream;

        private static ByteArrayStreamWriter tempWriter;

        static MessageCollection()
        {
            temp = new byte[MaxPacketSize];

            tempStream = new ByteArrayArrayStream(temp);

            tempWriter = new ByteArrayStreamWriter(tempStream);
        }

        private byte[] message;

        private ByteArrayArrayStream messageStream;

        private ByteArrayStreamWriter messageWriter;

        public MessageCollection()
        {
            message = ArrayPool<byte>.Shared.Rent(MaxMessageSize);

            messageStream = new ByteArrayArrayStream(message);

            messageWriter = new ByteArrayStreamWriter(messageStream);
        }

        private LinkedList<byte[]> messages;

        public void Add(IOutgoingPacket packet)
        {
            tempStream.Seek(Origin.Begin, 0);

            packet.Write(tempWriter);

            if (messageStream.Position + tempStream.Position > MaxMessageSize)
            {
                if (messages == null)
                {
                    messages = new LinkedList<byte[]>();
                }

                messages.AddLast(message[0 .. messageStream.Position] ); // Copy data to new array

                messageStream.Seek(Origin.Begin, 0);
            }

            messageWriter.Write(temp, 0, tempStream.Position);
        }

        public IEnumerable<byte[]> GetMessages()
        {
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    yield return message;
                }
            }

            yield return message[0 .. messageStream.Position]; // Copy data to new array
             
            messageStream.Seek(Origin.Begin, 0);
        }

        public void Dispose()
        {
            ArrayPool<byte>.Shared.Return(message);
        }
    }
}