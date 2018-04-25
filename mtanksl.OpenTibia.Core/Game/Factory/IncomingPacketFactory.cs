using OpenTibia.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenTibia
{
    public class IncomingPacketFactory
    {
        private Dictionary<byte, Type> packets = new Dictionary<byte, Type>();

        public IncomingPacketFactory()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(IIncomingPacket).IsAssignableFrom(type) ) )
            {
                PacketIdentifierAttribute attribute = type.GetCustomAttribute<PacketIdentifierAttribute>();

                if (attribute != null)
                {
                    packets.Add(attribute.Identifier, type);
                }
            }
        }
    
        public IIncomingPacket Create(byte identifier, ByteArrayStreamReader reader)
        {
            Type type;

            if ( !packets.TryGetValue(identifier, out type) )
            {
                return null;
            }

            IIncomingPacket packet = (IIncomingPacket)Activator.CreateInstance(type);

            packet.Read(reader);

            return packet;
        }

        public T Create<T>(ByteArrayStreamReader reader) where T : IIncomingPacket
        {
            T packet = Activator.CreateInstance<T>();

            packet.Read(reader);

            return packet;
        }
    }
}