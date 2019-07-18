using OpenTibia.IO;
using OpenTibia.Network.Packets.Incoming;
using System;

namespace OpenTibia.Game
{
    public class PacketsFactory
    {
        public T Create<T>(ByteArrayStreamReader reader) where T : IIncomingPacket
        {
            var packet = (IIncomingPacket)Activator.CreateInstance<T>();

            packet.Read(reader);

            return (T)packet;
        }
    }
}